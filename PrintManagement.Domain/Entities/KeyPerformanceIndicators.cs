using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Domain.Entities
{
    public class KeyPerformanceIndicators : BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public virtual User? Employee { get; set; }
        public string IndicatorName { get; set; }
        public int Target { get; set; }
        public int ActuallyAchieved { get; set; } = 0;
        public KPIEnum? Period { get; set; } = KPIEnum.Month;
        public bool? AchieveKPI { get; set; } = false;
    }
}
