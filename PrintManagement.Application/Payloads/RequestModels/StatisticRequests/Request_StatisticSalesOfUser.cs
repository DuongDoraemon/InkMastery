using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels.StatisticRequests
{
    public class Request_StatisticSalesOfUser
    {
        public int? Month { get; set; }
        public int? Quater { get; set; }    
        public int? Year { get; set; }
    }
}
