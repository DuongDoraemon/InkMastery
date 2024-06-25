using PrintManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.ResponseModels.DataResource
{
    public class DataResponseResourceProperty : DataResponseBase
    {
        public string ResourcePropertyName { get; set; }
        public int Quantity { get; set; }
        public IQueryable<DataResponseResourcePropertyDetail>? ResourcePropertyDetails { get; set; }
    }
}
