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
    public class PrintJobConverter
    {
        private readonly IBaseReposiroty<Design> _baseDesignRepository;
        private readonly IBaseReposiroty<ResourceForPrintJob> _repository;
        private readonly IBaseReposiroty<ResourcePropertyDetail> _propertyDetailRepository;
        private readonly ResourceForPrintJobConverter _converter;
        private readonly DesignConverter _designConverter;
        public PrintJobConverter(IBaseReposiroty<Design> baseDesignRepository, IBaseReposiroty<ResourceForPrintJob> repository, ResourceForPrintJobConverter converter, DesignConverter designConverter, IBaseReposiroty<ResourcePropertyDetail> propertyDetailRepository)
        {
            _baseDesignRepository = baseDesignRepository;
            _repository = repository;
            _converter = converter;
            _designConverter = designConverter;
            _propertyDetailRepository = propertyDetailRepository;
        }

        public DataResponsePrintJob EntityToDTO(PrintJob printJob)
        {
            var list = _repository.GetAllAsync(x => x.PrintJobId == printJob.Id).Result;
            List<string> result = new List<string>();
            
            foreach (var item in list)
            {
                var propertyDetail = _propertyDetailRepository.GetAllAsync(x => x.Id == item.ResourcePropertyDetailId);
                foreach(var property in propertyDetail.Result)
                {
                    result.Add(property.Name);
                }
            }
            return new DataResponsePrintJob
            {
                Id = printJob.Id,
                PrintJobStatus = printJob.PrintJobStatus.ToString(),
                DesignImage = _baseDesignRepository.GetAsync(x => x.Id == printJob.DesignId).Result.DesignImage,
                DesignId = printJob.DesignId,
                ResourceForPrints = result.AsQueryable(),
            };
        }
    }
}
