using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.OrderModule;
using Ecommerce.Domain.ProductModule;
using Ecommerce.Domain.UserModule;
using Ecommerce.ServiceAbstraction.IDashboardServices;
using Ecommerce.Shared.DashboardDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.DashboardServices
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DashboardDTO> GetDashboardDataAsync()
        {
            try
            {
                var users = await _unitOfWork.GetRepository<User>().GetAllAsync();
                int USERS_COUNT = users.Count();
                var products = await _unitOfWork.GetRepository<Product>().GetAllAsync();
                int PRODUCTS_COUNT = products.Count();
                var orders = await _unitOfWork.GetRepository<Order>().GetAllAsync();
                int ORDERS_COUNT = orders.Count();
                decimal REVENUE = orders.Sum(o => o.Total_Price);
                int PENDING_ORDERS = orders.Count(o => o.Status == OrderStatus.PendingPayment);
                int PROCESSED_ORDERS = orders.Count(o => o.Status == OrderStatus.Processing);
                int CANCELLED_ORDERS = orders.Count(o => o.Status == OrderStatus.Cancelled);
                DashboardDTO dashboard = new DashboardDTO()
                {
                    TotalProducts = PRODUCTS_COUNT,
                    TotalOrders = ORDERS_COUNT,
                    TotalRevenue = REVENUE,
                    totalCustomers = USERS_COUNT,
                    PendingOrders = PENDING_ORDERS,
                    ProcessedOrders = PROCESSED_ORDERS,
                    CancelledOrders = CANCELLED_ORDERS
                };
                return dashboard;
            }
            catch(Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"An error occurred while fetching dashboard data: {ex.Message}");
                // Optionally, you can rethrow the exception or return a default DashboardDTO
                throw; // Rethrow the exception to be handled by the caller
            }

        }
    }
}
