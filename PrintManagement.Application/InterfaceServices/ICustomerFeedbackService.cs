using PrintManagement.Application.Payloads.RequestModels;
using PrintManagement.Application.Payloads.ResponseModels.DataCustomerFeedback;
using PrintManagement.Application.Payloads.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.InterfaceServices
{
    public interface ICustomerFeedbackService
    {
        Task<ResponseObject<DataResponseCustomerFeedback>> CreateCustomerFeedback(Request_CreateCustomerFeedback request);
    }
}
