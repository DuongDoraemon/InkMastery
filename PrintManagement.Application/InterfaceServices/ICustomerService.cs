using PrintManagement.Application.Payloads.RequestModels.CustomerRequests;
using PrintManagement.Application.Payloads.RequestModels.InputRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataCustomer;
using PrintManagement.Application.Payloads.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.InterfaceServices
{
    public interface ICustomerService
    {
        Task<ResponseObject<DataResponseCustomer>> CreateCustomer(Request_CreateCustomer request);
        Task<ResponseObject<DataResponseCustomer>> UpdateCustomer(Request_UpdateCustomer request);
        Task<string> DeleteCustomer(Guid id);
        Task<IQueryable<DataResponseCustomer>> GetAllCustomers(Request_InputCustomer request);
        Task<DataResponseCustomer> GetCustomerById(Guid id);
    }
}
