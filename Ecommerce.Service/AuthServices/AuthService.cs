using AutoMapper;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.UserModule;
using Ecommerce.ServiceAbstraction.AuthServices;
using Ecommerce.ServiceAbstraction.IAuthServices;
using Ecommerce.Shared.UserDTOs;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            user.isActive = false;
            await _unitOfWork.SaveChanges();
        }

        public async Task<string> LoginAsync(LoginDTO dto)
        {
            var user = await _unitOfWork
                .GetRepository<User>()
                .GetByAttribute(u => u.Email == dto.Email);

            if (user is null)
                throw new UnauthorizedAccessException("Invalid email");

            if (!user.isActive)
                throw new UnauthorizedAccessException("User is inactive");

            return _tokenService.GenerateToken(user);
        }

        public async Task<string> RegisterAsync(RegisterDTO dto)
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

            return _tokenService.GenerateToken(user);
        }
    }
}
