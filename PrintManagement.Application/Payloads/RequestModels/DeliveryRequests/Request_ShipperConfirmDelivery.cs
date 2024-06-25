using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels.DeliveryRequests
{
    public class Request_ShipperConfirmDelivery
    {
        public Guid DeliveryId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ConfirmReceiptOfGoodsEnum? ConfirmStatus { get; set; }
    }
}
