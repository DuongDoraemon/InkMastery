using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Domain.Entities
{
    public class Design : BaseEntity
    {
        public Guid ProjectId { get; set; }
        public virtual Project? Project { get; set; }
        public Guid DesignerId { get; set; }
        public virtual User? Designer { get; set; }
        public string DesignImage { get; set; }
        public DateTime DesignTime { get; set; }
        public DesignStatusEnum? DesignStatus { get; set; }
        [MaybeNull]
        public Guid ApproverId { get; set; } // Người duyệt thiết kế
        public virtual ICollection<PrintJob>? PrintJobs { get; set; }
    }
}
