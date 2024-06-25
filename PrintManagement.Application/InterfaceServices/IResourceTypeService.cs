using PrintManagement.Application.Payloads.RequestModels.ResourceRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataResource;
using PrintManagement.Application.Payloads.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.InterfaceServices
{
    public interface IResourceTypeService
    {
        Task<ResponseObject<DataResponseResourceType>> CreateResourceType(Request_CreateResourceType request);
        Task<IQueryable<DataResponseResourceType>> GetAllResourceType();
        Task<DataResponseResourceType> GetResourceTypeById(Guid id);
    }
}
