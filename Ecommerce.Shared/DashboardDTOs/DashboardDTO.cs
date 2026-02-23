using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.DashboardDTOs
{
    public class DashboardDTO
    {
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int totalCustomers { get; set; }
        public int PendingOrders { get; set; }
        public int ProcessedOrders { get; set; }
        public int CancelledOrders { get; set; }
    }
}
