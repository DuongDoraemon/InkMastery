using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Domain.Entities
{
    public class ResourceForPrintJob : BaseEntity
    {
        public Guid ResourcePropertyDetailId { get; set; }
        public virtual ResourcePropertyDetail? Resource { get; set; }
        public int Quantity { get; set; }
        public Guid PrintJobId { get; set; }
        public virtual PrintJob? PrintJob { get; set; }
    }
}
