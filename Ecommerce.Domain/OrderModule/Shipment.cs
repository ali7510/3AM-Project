using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.OrderModule
{
    public class Shipment : BaseEntity
    {
        public string Company { get; set; } = string.Empty;
        public string TrackingNumber { get; set; } = string.Empty;
        public ShipmentStatus Status { get; set; }
        public DateTime Delivered_At { get; set; }

        #region Relationships
        public int Order_Id { get; set; }
        public Order Order { get; set; } = default!;
        #endregion

    }
}
