using PrintManagement.Application.Payloads.ResponseModels.DataResource;
using PrintManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.ResponseModels.DataPrintJob
{
    public class DataResponseResourceForPrintJob : DataResponseBase
    {
        public DataResponseResourcePropertyDetail Resource { get; set; }
        public int Quantity { get; set; }
    }
}
