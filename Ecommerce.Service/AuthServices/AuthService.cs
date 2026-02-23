using AutoMapper;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.UserModule;
using Ecommerce.ServiceAbstraction.AuthServices;
using Ecommerce.ServiceAbstraction.IAuthServices;
using Ecommerce.Shared.UserDTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ITokenService tokenService,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        public async Task<string> RegisterAsync(RegisterDTO dto)
        {
            try
            {
                var existingUser = await _unitOfWork
                    .GetRepository<User>()
                    .GetByAttribute(u => u.Email == dto.Email);

                bool isSamePhone = await _unitOfWork.GetRepository<User>()
                    .GetByAttribute(u => u.Phone == dto.Phone) != null;

                if (existingUser != null)
                    throw new InvalidOperationException("Email already exists");

                if (isSamePhone)
                    throw new InvalidOperationException("Phone number already exists");

                var user = _mapper.Map<User>(dto);
                await _unitOfWork.GetRepository<User>().AddAsync(user);
                await _unitOfWork.SaveChanges();

                return "Registered successfully. Please login with your email to receive an OTP.";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during registration: {ex.Message}");
            }

        }

        public async Task RequestOtpAsync(RequestOtpDTO dto)
        {
            try
            {
                var user = await _unitOfWork
                    .GetRepository<User>()
                    .GetByAttribute(u => u.Email == dto.Email);

                if (user is null)
                    throw new UnauthorizedAccessException("Email not found");

                if (!user.isActive)
                    throw new UnauthorizedAccessException("User is inactive");

                // Generate 6-digit OTP
                var otp = new Random().Next(100000, 999999).ToString();

                user.OtpCode = otp;
                user.OtpExpiry = DateTime.UtcNow.AddMinutes(5);

                _unitOfWork.GetRepository<User>().Update(user);
                await _unitOfWork.SaveChanges();

                await _emailService.SendOtpAsync(user.Email, otp);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error requesting OTP: {ex.Message}");
            }

        }

        public async Task<AuthResponseDTO> VerifyOtpAsync(VerifyOtpDTO dto)
        {
            try
            {
                var user = await _unitOfWork
                    .GetRepository<User>()
                    .GetByAttribute(u => u.Email == dto.Email);

                if (user is null)
                    throw new UnauthorizedAccessException("Email not found");

                if (user.OtpCode != dto.OtpCode)
                    throw new UnauthorizedAccessException("Invalid OTP");

                if (user.OtpExpiry < DateTime.UtcNow)
                    throw new UnauthorizedAccessException("OTP has expired");

                // Clear OTP
                user.OtpCode = null;
                user.OtpExpiry = null;

                // Generate tokens
                var accessToken = _tokenService.GenerateAccessToken(user);
                var refreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

                _unitOfWork.GetRepository<User>().Update(user);
                await _unitOfWork.SaveChanges();

                return new AuthResponseDTO
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiry = user.RefreshTokenExpiry.Value
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error verifying OTP: {ex.Message}");
            }
        }

        public async Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenDTO dto)
        {
            try
            {
                var user = await _unitOfWork
                    .GetRepository<User>()
                    .GetByAttribute(u => u.RefreshToken == dto.RefreshToken);

                if (user is null)
                    throw new UnauthorizedAccessException("Invalid refresh token");

                if (user.RefreshTokenExpiry < DateTime.UtcNow)
                    throw new UnauthorizedAccessException("Refresh token has expired, please login again");

                // Rotate refresh token
                var newAccessToken = _tokenService.GenerateAccessToken(user);
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

                _unitOfWork.GetRepository<User>().Update(user);
                await _unitOfWork.SaveChanges();

                return new AuthResponseDTO
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    RefreshTokenExpiry = user.RefreshTokenExpiry.Value
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error refreshing token: {ex.Message}");
            }

        }

        public async Task RevokeRefreshTokenAsync(int userId)
        {
            try
            {
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(userId);

                if (user is null)
                    throw new InvalidOperationException("User not found");

                user.RefreshToken = null;
                user.RefreshTokenExpiry = null;

                _unitOfWork.GetRepository<User>().Update(user);
                await _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error revoking refresh token: {ex.Message}");
            }

        }

        public async Task DeleteUser(int userId)
        {
            try
            {
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(userId);

                if (user == null)
                    throw new InvalidOperationException("User not found");

                user.isActive = false;
                _unitOfWork.GetRepository<User>().Update(user);
                await _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting user: {ex.Message}");
            }

        }
    }
}
