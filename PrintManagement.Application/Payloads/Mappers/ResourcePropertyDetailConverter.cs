using PrintManagement.Application.Payloads.ResponseModels.DataResource;
using PrintManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.Mappers
{
    public class ResourcePropertyDetailConverter
    {
        public DataResponseResourcePropertyDetail EntityToDTO(ResourcePropertyDetail detail)
        {
            return new DataResponseResourcePropertyDetail
            {
                Id = detail.Id,
                Name = detail.Name,
                Price = detail.Price,
                Image = detail.Image,
                Quantity = detail.Quantity
            };
        }
    }
}
