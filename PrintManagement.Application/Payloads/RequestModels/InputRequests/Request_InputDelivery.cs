using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels.InputRequests
{
    public class Request_InputDelivery
    {
        public Guid? ProjectId { get; set; }
        public Guid? DeliverId { get; set; }
        public Guid? CustomerId { get; set; }
        public DeliveryStatusEnum? DeliveryStatus { get; set; }
    }
}
