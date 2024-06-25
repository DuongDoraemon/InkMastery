using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels.DesignRequests
{
    public class Request_UpdateDesign
    {
        public Guid DesignId { get; set; }
        public Guid ProjectId { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile DesignImage { get; set; }
    }
}
