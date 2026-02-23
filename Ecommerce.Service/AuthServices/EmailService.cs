using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.ServiceAbstraction.IAuthServices;

namespace Ecommerce.Service.AuthServices
{
    public class EmailService: IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendOtpAsync(string toEmail, string otpCode)
        {
            var smtpHost = _configuration["Email:SmtpHost"]!;
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"]!);
            var smtpUser = _configuration["Email:Username"]!;
            var smtpPass = _configuration["Email:Password"]!;
            var fromEmail = _configuration["Email:From"]!;

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            var mail = new MailMessage(fromEmail, toEmail)
            {
                Subject = "Your OTP Code",
                Body = $"Your OTP code is: {otpCode}\nIt expires in 5 minutes.",
                IsBodyHtml = false
            };

            await client.SendMailAsync(mail);
        }
    }
}
