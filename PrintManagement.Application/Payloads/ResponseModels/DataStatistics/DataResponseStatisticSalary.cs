using PrintManagement.Application.Payloads.ResponseModels.DataUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.ResponseModels.DataStatistics
{
    public class DataResponseStatisticSalary
    {
        public DataResponseUser User { get; set; }
        public decimal Salary { get; set; }
        public int Month { get; set; }
    }
}
