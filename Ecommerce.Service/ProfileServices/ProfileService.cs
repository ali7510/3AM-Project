using AutoMapper;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.OrderModule;
using Ecommerce.Domain.UserModule;
using Ecommerce.ServiceAbstraction.IProfileServices;
using Ecommerce.Shared.ProfileDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.ProfileServices
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProfileService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        public async Task<ProfileDTO> GetUserProfile(int userId)
        {
            try
            {
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(userId);
                if (user == null)
                    throw new InvalidOperationException("User not found");

                if (user.isActive == false)
                    throw new UnauthorizedAccessException("User is inactive");

                return _mapper.Map<ProfileDTO>(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user profile: {ex.Message}");
            }
        }
    }
}
