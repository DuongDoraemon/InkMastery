using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Cmp;
using PrintManagement.Application.Handle.HandleFile;
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
    public class ResourceService : IResourceService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBaseReposiroty<Resource> _baseResourceRepository;
        private readonly ResourceConverter _converter;
        private readonly IBaseReposiroty<ResourceProperty> _baseResourcePropertyRepository;
        private readonly ResourcePropertyConverter _resourcePropertyConverter;
        private readonly IBaseReposiroty<ResourcePropertyDetail> _baseResourcePropertyDetailRepository;
        private readonly IBaseReposiroty<ResourceType> _resourceTypeRepository;
        public ResourceService(IHttpContextAccessor contextAccessor, IBaseReposiroty<Resource> baseResourceRepository, ResourceConverter converter, IBaseReposiroty<ResourceProperty> baseResourcePropertyRepository, ResourcePropertyConverter resourcePropertyConverter, IBaseReposiroty<ResourcePropertyDetail> baseResourcePropertyDetailRepository, IBaseReposiroty<ResourceType> resourceTypeRepository)
        {
            _contextAccessor = contextAccessor;
            _baseResourceRepository = baseResourceRepository;
            _converter = converter;
            _baseResourcePropertyRepository = baseResourcePropertyRepository;
            _resourcePropertyConverter = resourcePropertyConverter;
            _baseResourcePropertyDetailRepository = baseResourcePropertyDetailRepository;
            _resourceTypeRepository = resourceTypeRepository;
        }
        public async Task<ResponseObject<DataResponseResource>> CreateResourceInformation(Request_CreateResource request)
        {
            var currentUser = _contextAccessor.HttpContext.User;
            try
            {
                if(!currentUser.Identity.IsAuthenticated)
                {
                    return new ResponseObject<DataResponseResource>
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Message = "UnAuthenticated user",
                        Data = null
                    };
                }
                if (!currentUser.IsInRole("Admin"))
                {
                    return new ResponseObject<DataResponseResource>
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Message = "You do not have permission to perform this function",
                        Data = null
                    };
                }
                var resourceType = await _resourceTypeRepository.GetByIDAsync(request.ResourceTypeId);
                if(resourceType == null)
                {
                    return new ResponseObject<DataResponseResource>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Loại tài nguyên không tồn tại",
                        Data = null
                    };
                }
                var resource = new Resource
                {
                    AvailableQuantity = 0,
                    IsActive = true,
                    Id = Guid.NewGuid(),
                    Image = await HandleUploadFile.Upfile(request.Image),
                    ResourceName = request.ResourceName,
                    ResourceProperties = null,
                    ResourceTypeId = request.ResourceTypeId,
                    ResourceStatus = Domain.Enumerates.ResourceStatusEnum.OutOfStock
                };
                resource = await _baseResourceRepository.CreateAsync(resource);
                return new ResponseObject<DataResponseResource>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Resource created successfully",
                    Data = _converter.EntityToDTO(resource)
                };
            }catch (Exception ex)
            {
                return new ResponseObject<DataResponseResource>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<string> DeleteResource(Guid resourceId)
        {
            var currentUser = _contextAccessor.HttpContext.User;
            try
            {
                if (!currentUser.Identity.IsAuthenticated)
                {
                    return "UnAuthenticated user";
                }
                if (!currentUser.IsInRole("Admin"))
                {
                    return "You do not have permission to perform this function";
                }
                var resource = await _baseResourceRepository.GetByIDAsync(resourceId);
                if (resource == null)
                {
                    return "Resource not found";
                }
                resource.IsActive = false;
                await _baseResourceRepository.UpdateAsync(resource);
                return "Resource deleted successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<IQueryable<DataResponseResource>> GetAll(string? resourceName)
        {
            var query = await _baseResourceRepository.GetAllAsync(record => record.IsActive == true);
            if(!string.IsNullOrEmpty(resourceName))
            {
                query = query.Where(x => x.ResourceName.ToLower().Contains(resourceName.ToLower()));
            }
            return query.Select(x => _converter.EntityToDTO(x));
        }

        public async Task<DataResponseResource> GetById(Guid resourceId)
        {
            return _converter.EntityToDTO(await _baseResourceRepository.GetByIDAsync(resourceId));
        }

        public async Task<ResponseObject<DataResponseResource>> UpdateResourceInformation(Request_UpdateResource request)
        {
            var currentUser = _contextAccessor.HttpContext.User;
            try
            {
                if (!currentUser.Identity.IsAuthenticated)
                {
                    return new ResponseObject<DataResponseResource>
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Message = "UnAuthenticated user",
                        Data = null
                    };
                }
                if (!currentUser.IsInRole("Admin"))
                {
                    return new ResponseObject<DataResponseResource>
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Message = "You do not have permission to perform this function",
                        Data = null
                    };
                }
                var resource = await _baseResourceRepository.GetByIDAsync(request.Id);
                if (resource == null)
                {
                    return new ResponseObject<DataResponseResource>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Resource not found",
                        Data = null
                    };
                }
                resource.ResourceName = resource.ResourceName;
                resource.Image = await HandleUploadFile.Upfile(request.Image);
                await _baseResourceRepository.UpdateAsync(resource);
                return new ResponseObject<DataResponseResource>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Resource updated successfully",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ResponseObject<DataResponseResource>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Data = null
                };
            }
        }


        #region Private methods
        public async Task<ResponseObject<DataResponseResource>> CreateResourcePropertyAsync(Request_CreateResourceProperty request)
        {
            try
            {
                var resource = await _baseResourceRepository.GetByIDAsync(request.ResourceId);
                if (resource == null)
                {
                    return new ResponseObject<DataResponseResource>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Tài nguyên không tồn tại",
                        Data = null
                    };
                }
                List<ResourceProperty> resourceProperties = new List<ResourceProperty>();
                var propertyItem = new ResourceProperty
                {
                    IsActive = true,
                    Id = Guid.NewGuid(),
                    Quantity = 0,
                    ResourceId = request.ResourceId,
                    ResourcePropertyDetails = null,
                    ResourcePropertyName = request.ResourcePropertyName
                };
                resourceProperties.Add(propertyItem);
                propertyItem = await _baseResourcePropertyRepository.CreateAsync(propertyItem);
                propertyItem.ResourcePropertyDetails = CreateResourcePropertyDetailAsync(propertyItem.Id, request.requests).Result.ToList();
                resource.ResourceProperties = resourceProperties;
                await _baseResourceRepository.UpdateAsync(resource);
                return new ResponseObject<DataResponseResource> 
                { 
                    Status = StatusCodes.Status200OK,
                    Message = "Thêm thuộc tính tài nguyên thành công",
                    Data = _converter.EntityToDTO(resource)
                };
            }catch (Exception ex)
            {
                return new ResponseObject<DataResponseResource>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<string> DeleteResourcePropertyDetail(Guid id)
        {
            var resourceProperty = await _baseResourcePropertyDetailRepository.GetByIDAsync(id);
            resourceProperty.IsActive = false;
            await _baseResourcePropertyDetailRepository.UpdateAsync(resourceProperty);
            return "Xóa bản ghi thành công";
        }

        private async Task<IQueryable<ResourcePropertyDetail>> CreateResourcePropertyDetailAsync(Guid resourcePropertyId, IEnumerable<Request_CreateResourcePropertyDetail> requets)
        {
            try
            {
                var resourceProperty = await _baseResourcePropertyRepository.GetByIDAsync(resourcePropertyId);
                var resource = await _baseResourceRepository.GetByIDAsync(resourceProperty.ResourceId);
                if (resourceProperty == null)
                {
                    throw new ArgumentNullException("Không tìm thấy thuộc tính tài nguyên");
                }
                List<ResourcePropertyDetail> listResult = new List<ResourcePropertyDetail>();
                foreach(var resourcePropertyDetail in requets)
                {
                    var item = new ResourcePropertyDetail
                    {
                        IsActive= true,
                        Id = Guid.NewGuid(),
                        Name = resourcePropertyDetail.Name,
                        Price = resourcePropertyDetail.Price,
                        Quantity = 0,
                        Image = resource.Image,
                        ResourcePropertyId = resourcePropertyId
                    };
                    listResult.Add(item);
                    item = await _baseResourcePropertyDetailRepository.CreateAsync(item);
                }
                resourceProperty.ResourcePropertyDetails = listResult;
                await _baseResourcePropertyRepository.UpdateAsync(resourceProperty);
                return listResult.AsQueryable();
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
