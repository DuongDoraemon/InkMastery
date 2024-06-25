using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.ResponseModels.DataRole
{
    public class DataResponseRole : DataResponseBase
    {
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
    }
}
