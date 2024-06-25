using PrintManagement.Application.Payloads.ResponseModels.DataPrintJob;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.Mappers
{
    public class ResourceForPrintJobConverter
    {
        private readonly IBaseReposiroty<ResourcePropertyDetail> _repository;
        private readonly ResourcePropertyDetailConverter _converter;
        public ResourceForPrintJobConverter(IBaseReposiroty<ResourcePropertyDetail> repository, ResourcePropertyDetailConverter converter)
        {
            _repository = repository;
            _converter = converter;
        }

        public DataResponseResourceForPrintJob EntityToDTO(ResourceForPrintJob resourceForPrintJob)
        {
            return new DataResponseResourceForPrintJob
            {
                Id = resourceForPrintJob.Id,
                Resource = _converter.EntityToDTO(_repository.GetAsync(x => x.Id == resourceForPrintJob.ResourcePropertyDetailId).Result),
                Quantity = resourceForPrintJob.Quantity,
            };
        }
    }
}
