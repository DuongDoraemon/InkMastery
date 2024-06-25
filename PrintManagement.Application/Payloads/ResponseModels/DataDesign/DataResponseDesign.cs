using PrintManagement.Application.Payloads.ResponseModels.DataUser;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.ResponseModels.DataDesign
{
    public class DataResponseDesign : DataResponseBase
    {
        public string Designer { get; set; }
        public string DesignImage { get; set; }
        public DateTime DesignTime { get; set; }
        public string DesignStatus { get; set; }
        public string? Approver { get; set; } // Người duyệt thiết kế
    }
}
