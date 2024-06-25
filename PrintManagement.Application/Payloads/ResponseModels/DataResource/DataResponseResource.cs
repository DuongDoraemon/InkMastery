using PrintManagement.Domain.Entities;
using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.ResponseModels.DataResource
{
    public class DataResponseResource : DataResponseBase
    {
        public string ResourceName { get; set; }
        public string ResourceTypeName { get; set; }
        public int AvailableQuantity { get; set; }
        public string Image { get; set; }
        public string ResourceStatus { get; set; }
        public IQueryable<DataResponseResourceProperty>? ResourceProperties { get; set; }
    }
}
