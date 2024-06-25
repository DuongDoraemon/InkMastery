using Microsoft.AspNetCore.Http;
using PrintManagement.Application.InterfaceServices;
using PrintManagement.Application.Payloads.Mappers;
using PrintManagement.Application.Payloads.RequestModels.ResourceRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataResource;
using PrintManagement.Application.Payloads.Responses;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.ImplementServices
{
    public class ResourceTypeService : IResourceTypeService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBaseReposiroty<Resource> _resourceRepository;
        private readonly IBaseReposiroty<ResourceType> _resourceTypeRepository;
        private readonly ResourceTypeConverter _resourceTypeConverter;
        public ResourceTypeService(IHttpContextAccessor contextAccessor, IBaseReposiroty<Resource> resourceRepository, IBaseReposiroty<ResourceType> resourceTypeRepository, ResourceTypeConverter resourceTypeConverter)
        {
            _contextAccessor = contextAccessor;
            _resourceRepository = resourceRepository;
            _resourceTypeRepository = resourceTypeRepository;
            _resourceTypeConverter = resourceTypeConverter;
        }

        public async Task<ResponseObject<DataResponseResourceType>> CreateResourceType(Request_CreateResourceType request)
        {
            var currentUser = _contextAccessor.HttpContext.User;
            try
            {
                if (!currentUser.Identity.IsAuthenticated)
                {
                    return new ResponseObject<DataResponseResourceType>
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Message = "Người dùng chưa được xác thực",
                        Data = null
                    };
                }
                if (!currentUser.IsInRole("Admin"))
                {
                    return new ResponseObject<DataResponseResourceType>
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Message = "Bạn không có quyền thực hiện chức năng này",
                        Data = null
                    };
                }
                ResourceType resourceType = new ResourceType
                {
                    IsActive = true,
                    Id = Guid.NewGuid(),
                    NameOfResourceType = request.NameOfResourceType
                };
                resourceType = await _resourceTypeRepository.CreateAsync(resourceType);
                return new ResponseObject<DataResponseResourceType>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Tạo loại tài nguyên thành công",
                    Data = _resourceTypeConverter.EntityToDTO(resourceType)
                };
            }catch (Exception ex)
            {
                return new ResponseObject<DataResponseResourceType>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<IQueryable<DataResponseResourceType>> GetAllResourceType()
        {
            var query = _resourceTypeRepository.GetAllAsync(x => x.IsActive == true).Result.Select(x => _resourceTypeConverter.EntityToDTO(x));
            return query;
        }

        public async Task<DataResponseResourceType> GetResourceTypeById(Guid id)
        {
            return _resourceTypeConverter.EntityToDTO(await _resourceTypeRepository.GetByIDAsync(id));
        }
    }
}
