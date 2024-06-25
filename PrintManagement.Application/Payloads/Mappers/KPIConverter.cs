using PrintManagement.Application.Payloads.ResponseModels.DataKPI;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.Mappers
{
    public class KPIConverter
    {
        private readonly IBaseReposiroty<User> _repository;
        private readonly UserConverter _converter;
        public KPIConverter(IBaseReposiroty<User> repository, UserConverter converter)
        {
            _repository = repository;
            _converter = converter;
        }
        public DataResponseKPI EntityToDTO(KeyPerformanceIndicators key)
        {
            return new DataResponseKPI
            {
                AchieveKPI = key.AchieveKPI.ToString(),
                ActuallyAchieved = key.ActuallyAchieved,
                Employee = _converter.EntityToDTOForUser(_repository.GetAsync(x => x.Id == key.EmployeeId).Result),
                Id = key.Id,
                IndicatorName = key.IndicatorName,
                Period = key.Period.ToString(),
                Target = key.Target,
            };
        }
    }
}
