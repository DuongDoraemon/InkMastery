using PrintManagement.Application.Payloads.ResponseModels.DataUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.ResponseModels.DataStatistics
{
    public class DataResponseStatisticSalesOfUser
    {
        public DataResponseUser DataResponseUser { get; set; }
        public DataResponseStatisticSales StatisticSales { get; set; }
    }
}
