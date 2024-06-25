using PrintManagement.Application.Payloads.RequestModels.DeliveryRequests;
using PrintManagement.Application.Payloads.RequestModels.InputRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataDelivery;
using PrintManagement.Application.Payloads.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.InterfaceServices
{
    public interface IDeliveryService
    {
        Task<ResponseObject<DataResponseDelivery>> CreateDelivery(Request_CreateDelivery request);
        Task<ResponseObject<DataResponseDelivery>> ShipperConfirmOrderDelivery(Guid shipperId, Request_ShipperConfirmOrderDelivery request);
        Task<ResponseObject<DataResponseDelivery>> ShipperConfirmDelivery(Guid shipperId, Request_ShipperConfirmDelivery request);
        Task<ResponseObject<DataResponseDelivery>> CustomerConfirmDelivery(Guid deliveryId);
        Task<IQueryable<DataResponseDelivery>> GetAllDelivery(Request_InputDelivery input);
        Task<DataResponseDelivery> GetDeliveryById(Guid id);
    }
}
