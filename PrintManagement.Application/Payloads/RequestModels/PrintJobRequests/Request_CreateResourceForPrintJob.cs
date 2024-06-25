using PrintManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels.PrintJobRequests
{
    public class Request_CreateResourceForPrintJob
    {
        public Guid ResourcePropertyDetailId { get; set; }
        public int Quantity { get; set; }
    }
}
