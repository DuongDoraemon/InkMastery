using PrintManagement.Domain.Entities;
using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels.PrintJobRequests
{
    public class Request_CreatePrintJob
    {
        public Guid DesignId { get; set; }
        public List<Request_CreateResourceForPrintJob>? ResourceForPrints { get; set; }
    }
}
