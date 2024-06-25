using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Domain.Entities
{
    public class ResourceProperty : BaseEntity
    {
        public Guid ResourceId { get; set; }
        public virtual Resource? Resource { get; set; }
        public string ResourcePropertyName { get; set; }
        public int Quantity { get; set; }
        public virtual ICollection<ResourcePropertyDetail>? ResourcePropertyDetails { get; set; }
    }
}
