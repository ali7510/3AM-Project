using Ecommerce.Shared.DashboardDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstraction.IDashboardServices
{
    public interface IDashboardService
    {
        Task<DashboardDTO> GetDashboardDataAsync();
    }
}
