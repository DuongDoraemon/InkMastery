using PrintManagement.Application.Payloads.ResponseModels.DataResource;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.Mappers
{
    public class ResourceTypeConverter
    {
        private readonly IBaseReposiroty<Resource> _repository;
        private readonly ResourceConverter _converter;
        public ResourceTypeConverter(IBaseReposiroty<Resource> repository, ResourceConverter converter)
        {
            _repository = repository;
            _converter = converter;
        }
        public DataResponseResourceType EntityToDTO(ResourceType resourceType)
        {
            return new DataResponseResourceType
            {
                Id = resourceType.Id,
                NameOfResourceType = resourceType.NameOfResourceType,
                Resources = _repository.GetAllAsync(x => x.ResourceTypeId == resourceType.Id).Result.Select(x => _converter.EntityToDTO(x))
            };
        }
    }
}
