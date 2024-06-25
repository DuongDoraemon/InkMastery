using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels.KPIRequests
{
    public class Request_UpdateKPI
    {
        public Guid KpiId { get; set; }
        public Guid EmployeeId { get; set; }
        public string IndicatorName { get; set; }
        public int Target { get; set; }
        public KPIEnum? Period { get; set; } = KPIEnum.Month;
    }
}
