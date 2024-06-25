using Microsoft.AspNetCore.Http;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels.DesignRequests
{
    public class Request_CreateDesign
    {
        public Guid ProjectId { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile DesignImage { get; set; }
    }
}
