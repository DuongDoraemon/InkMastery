using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Domain.Entities
{
    public class Resource : BaseEntity
    {
        public Guid ResourceTypeId { get; set; }
        public virtual ResourceType? ResourceType { get; set; }
        public string ResourceName { get; set; }
        public int AvailableQuantity { get; set; }
        public string Image { get; set; }
        public ResourceStatusEnum? ResourceStatus { get; set; } = ResourceStatusEnum.ReadyToUse;
        public virtual ICollection<ResourceProperty>? ResourceProperties { get; set; }
    }
}
