using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Domain.Entities
{
    public class Team : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? NumberOfMember { get; set; } = 0;
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        [MaybeNull]
        public Guid ManagerId { get; set; }
        public virtual ICollection<User>? Users { get; set; }
    }
}
