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
    public class ResourcePropertyConverter
    {
        private readonly ResourcePropertyDetailConverter _converter;
        private readonly IBaseReposiroty<ResourcePropertyDetail> _reposiroty;
        public ResourcePropertyConverter(ResourcePropertyDetailConverter converter, IBaseReposiroty<ResourcePropertyDetail> reposiroty)
        {
            _converter = converter;
            _reposiroty = reposiroty;
        }
        public DataResponseResourceProperty EntityToDTO(ResourceProperty resourceProperty)
        {
            return new DataResponseResourceProperty
            {
                Id = resourceProperty.Id,
                Quantity = resourceProperty.Quantity,
                ResourcePropertyName = resourceProperty.ResourcePropertyName,
                ResourcePropertyDetails = _reposiroty.GetAllAsync(x => x.ResourcePropertyId == resourceProperty.Id && x.IsActive == true).Result.Select(x => _converter.EntityToDTO(x))
            };
        }
    }
}
