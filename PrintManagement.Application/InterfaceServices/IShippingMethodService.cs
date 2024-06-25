using PrintManagement.Application.Payloads.RequestModels.ShippingMethodRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataShippingMethod;
using PrintManagement.Application.Payloads.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.InterfaceServices
{
    public interface IShippingMethodService
    {
        Task<ResponseObject<DataResponseShippingMethod>> CreateShippingMethod(Request_CreateShippingMethod request);
    }
}
