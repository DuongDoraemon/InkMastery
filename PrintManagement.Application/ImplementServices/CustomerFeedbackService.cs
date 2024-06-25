using PrintManagement.Application.InterfaceServices;
using PrintManagement.Application.Payloads.RequestModels;
using PrintManagement.Application.Payloads.ResponseModels.DataCustomerFeedback;
using PrintManagement.Application.Payloads.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.ImplementServices
{
    public class CustomerFeedbackService : ICustomerFeedbackService
    {
        public Task<ResponseObject<DataResponseCustomerFeedback>> CreateCustomerFeedback(Request_CreateCustomerFeedback request)
        {
            throw new NotImplementedException();
        }
    }
}
