using PrintManagement.Application.Payloads.ResponseModels.DataUser;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.ResponseModels.DataKPI
{
    public class DataResponseKPI : DataResponseBase
    {
        public DataResponseUser Employee { get; set; }
        public string IndicatorName { get; set; }
        public int Target { get; set; }
        public int ActuallyAchieved { get; set; }
        public string Period { get; set; }
        public string AchieveKPI { get; set; }
    }
}
