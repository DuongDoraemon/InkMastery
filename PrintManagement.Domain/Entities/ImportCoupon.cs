using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Domain.Entities
{
    public class ImportCoupon : BaseEntity
    {
        public decimal TotalMoney { get; set; }
        public Guid ResourcePropertyDetailId { get; set; }
        public virtual ResourcePropertyDetail? ResourcePropertyDetail { get; set; }
        public Guid EmployeeId { get; set; }
        public virtual User? Employee { get; set; }
        public int Quantity { get; set; }
        public string TradingCode { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
