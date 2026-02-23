using Ecommerce.Shared.ProfileDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstraction.IProfileServices
{
    public interface IProfileService
    {
        Task<ProfileDTO> GetUserProfile(int userId);
        Task DeleteUser(int userId);
    }
}
