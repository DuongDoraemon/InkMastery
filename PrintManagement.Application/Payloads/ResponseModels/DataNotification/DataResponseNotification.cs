using PrintManagement.Application.Payloads.ResponseModels.DataUser;
using PrintManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.ResponseModels.DataNotification
{
    public class DataResponseNotification : DataResponseBase
    {
        public string Content { get; set; }
        public string Link { get; set; }
        public bool? IsSeen { get; set; }
    }
}
