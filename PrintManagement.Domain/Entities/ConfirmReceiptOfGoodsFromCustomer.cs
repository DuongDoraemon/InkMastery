using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Domain.Entities
{
    public class ConfirmReceiptOfGoodsFromCustomer : BaseEntity
    {
        public Guid DeliveryId { get; set; }
        public virtual Delivery? Delivery { get; set; }
        public ConfirmReceiptOfGoodsEnum? ConfirmReceiptOfGoods { get; set; } = ConfirmReceiptOfGoodsEnum.NotReceived;
    }
}
