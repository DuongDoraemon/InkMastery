using PrintManagement.Domain.Entities;
using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels.DeliveryRequests
{
    public class Request_CreateDelivery
    {
        public Guid ShippingMethodId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid DeliverId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime EstimateDeliveryTime { get; set; }
    }
}
