using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Domain.Entities
{
    public class PrintJob : BaseEntity
    {
        public Guid DesignId { get; set; }
        public virtual Design? Design { get; set; }
        public PrintJobStatusEnum? PrintJobStatus { get; set; }
        public virtual ICollection<ResourceForPrintJob>? ResourceForPrints { get; set; }
    }
}
