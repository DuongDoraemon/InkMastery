using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels.TeamRequests
{
    public class Request_ChangeDepartmentForUser
    {
        public Guid EmployeeId { get; set; }
        public Guid TeamId { get; set; }
    }
}
