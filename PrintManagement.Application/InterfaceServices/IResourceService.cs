using PrintManagement.Application.Payloads.RequestModels.ResourceRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataResource;
using PrintManagement.Application.Payloads.Responses;
using PrintManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.InterfaceServices
{
    public interface IResourceService
    {
        Task<ResponseObject<DataResponseResource>> CreateResourceInformation(Request_CreateResource request);
        Task<ResponseObject<DataResponseResource>> UpdateResourceInformation(Request_UpdateResource request);
        Task<string> DeleteResource(Guid resourceId);
        Task<IQueryable<DataResponseResource>> GetAll(string? resourceName);
        Task<DataResponseResource> GetById(Guid resourceId);
        Task<ResponseObject<DataResponseResource>> CreateResourcePropertyAsync(Request_CreateResourceProperty request);
        Task<string> DeleteResourcePropertyDetail(Guid id);
    }
}
