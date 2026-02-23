using AutoMapper;
using Ecommerce.Domain.CartModule;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.OrderModule;
using Ecommerce.Domain.UserModule;
using Ecommerce.ServiceAbstraction.IPaymentServices;
using Ecommerce.Shared.OrderDTOs;
using Ecommerce.Shared.PaymentDTOs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly MyFatoorahSettings _settings;

        public PaymentService(
            IHttpClientFactory httpClientFactory,
            IUnitOfWork unitOfWork,
            IOptions<MyFatoorahSettings> settings)
        {
            _httpClientFactory = httpClientFactory;
            _unitOfWork = unitOfWork;
            _settings = settings.Value;
        }

        public async Task<PaymentResponseDTO> ConfirmPaymentAsync(int userId, PaymentMethod method)
        {
            // Step A: Get cart with items and products
            var cartRepo = _unitOfWork.GetRepository<Cart>();
            var cart = await cartRepo.GetByAttribute(c => c.User_Id == userId);

            if (cart == null)
                throw new Exception("Cart not found.");

            var cartWithItems = await cartRepo.GetByIdAsync(cart.Id,
                c => c.CartItems,
                c => c.User);

            // We need CartItems with Products included — use GetAllAsync
            var cartItemRepo = _unitOfWork.GetRepository<CartItem>();
            var cartItems = await cartItemRepo.GetAllAsync(
                ci => ci.Cart_Id == cart.Id,
                ci => ci.Product);

            if (!cartItems.Any())
                throw new Exception("Cart is empty.");

            // Step B: Calculate total
            var total = cartItems.Sum(ci => ci.Quantity * ci.Product.Price);

            // Step C: Get user
            var userRepo = _unitOfWork.GetRepository<User>();
            var user = await userRepo.GetByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found.");

            // Step D: Create Order
            var order = new Order
            {
                User_Id = userId,
                Total_Price = total,
                Status = OrderStatus.PendingPayment,
                Payment_Status = PaymentStatus.Pending,
                OrderItems = cartItems.Select(ci => new OrderItem
                {
                    Product_Id = ci.Product_Id,
                    Quantity = ci.Quantity,
                    Price_At_Purchase = ci.Product.Price
                }).ToList()
            };

            var orderRepo = _unitOfWork.GetRepository<Order>();
            await orderRepo.AddAsync(order);

            // Step E: Create Payment record
            var payment = new Payment
            {
                Order = order,
                Amount = total,
                Method = method,
                Status = PaymentStatus.Pending
            };

            var paymentRepo = _unitOfWork.GetRepository<Payment>();
            await paymentRepo.AddAsync(payment);

            // Step F: Call MyFatoorah to create invoice
            var client = _httpClientFactory.CreateClient("MyFatoorah");

            var invoiceRequest = new
            {
                CustomerName = user.Name,
                NotificationOption = "ALL",
                InvoiceValue = total,
                CallbackUrl = _settings.CallbackUrl,
                ErrorUrl = _settings.ErrorUrl
            };

            var response = await client.PostAsJsonAsync("/v2/SendPayment", invoiceRequest);

            if (!response.IsSuccessStatusCode)
            {
                // Don't save anything — invoice creation failed
                throw new Exception("Failed to create MyFatoorah invoice.");
            }

            var responseContent = await response.Content.ReadFromJsonAsync<MyFatoorahSendPaymentResponse>();

            if (responseContent?.IsSuccess != true || responseContent.Data == null)
                throw new Exception("MyFatoorah returned an unsuccessful response.");

            // Step G: Store invoice data
            payment.ExternalPaymentId = responseContent.Data.InvoiceId.ToString();
            payment.PaymentURL = responseContent.Data.InvoiceURL;

            // Step H: Save everything
            await _unitOfWork.SaveChanges();

            // Step I: Clear the cart
            var cartItemsToRemove = cartItems.ToList();
            foreach (var item in cartItemsToRemove)
                cartItemRepo.Remove(item);

            await _unitOfWork.SaveChanges();

            return new PaymentResponseDTO
            {
                Success = true,
                PaymentUrl = responseContent.Data.InvoiceURL,
                ExternalPaymentId = responseContent.Data.InvoiceId.ToString(),
                RequiresRedirect = true
            };
        }

        public async Task HandleCallbackAsync(string invoiceId)
        {
            // Step A: Find payment by ExternalPaymentId
            var paymentRepo = _unitOfWork.GetRepository<Payment>();
            var payment = await paymentRepo.GetByAttribute(p => p.ExternalPaymentId == invoiceId);

            if (payment == null)
                throw new Exception($"Payment with InvoiceId {invoiceId} not found.");

            // Load the related order
            var orderRepo = _unitOfWork.GetRepository<Order>();
            var order = await orderRepo.GetByIdAsync(payment.Order_Id);

            if (order == null)
                throw new Exception("Order not found.");

            // Step B: Verify with MyFatoorah
            var client = _httpClientFactory.CreateClient("MyFatoorah");

            var verifyRequest = new
            {
                Key = invoiceId,
                KeyType = "InvoiceId"
            };

            var response = await client.PostAsJsonAsync("/v2/GetPaymentStatus", verifyRequest);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to verify payment with MyFatoorah.");

            var result = await response.Content.ReadFromJsonAsync<MyFatoorahPaymentStatusResponse>();

            // Step C: Update based on result
            if (result?.Data?.InvoiceStatus == "Paid")
            {
                payment.Status = PaymentStatus.Paid;
                payment.Created_At = DateTime.UtcNow;
                order.Status = OrderStatus.Processing;
                order.Payment_Status = PaymentStatus.Paid;
            }
            else
            {
                payment.Status = PaymentStatus.Failed;
                order.Status = OrderStatus.Cancelled;
                order.Payment_Status = PaymentStatus.Failed;
            }

            paymentRepo.Update(payment);
            orderRepo.Update(order);
            await _unitOfWork.SaveChanges();
        }
    }

    public class MyFatoorahSendPaymentResponse
    {
        public bool IsSuccess { get; set; }
        public SendPaymentData? Data { get; set; }
    }

    public class SendPaymentData
    {
        public int InvoiceId { get; set; }
        public string InvoiceURL { get; set; } = string.Empty;
    }

    public class MyFatoorahPaymentStatusResponse
    {
        public bool IsSuccess { get; set; }
        public PaymentStatusData? Data { get; set; }
    }

    public class PaymentStatusData
    {
        public string InvoiceStatus { get; set; } = string.Empty;
    }
}
