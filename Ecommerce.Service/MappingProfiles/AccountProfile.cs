using AutoMapper;
using Ecommerce.Domain.UserModule;
using Ecommerce.Shared.ProfileDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.MappingProfiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<User, ProfileDTO>();
        }
    }
}
