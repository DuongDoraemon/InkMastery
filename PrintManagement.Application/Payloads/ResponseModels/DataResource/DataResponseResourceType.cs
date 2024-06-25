using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.ResponseModels.DataResource
{
    public class DataResponseResourceType : DataResponseBase
    {
        public string NameOfResourceType { get; set; }
        public IQueryable<DataResponseResource> Resources { get; set; }
    }
}
