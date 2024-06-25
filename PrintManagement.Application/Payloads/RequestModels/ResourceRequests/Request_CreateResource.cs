using Microsoft.AspNetCore.Http;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels.ResourceRequests
{
    public class Request_CreateResource
    {
        [Required(ErrorMessage = "ResourceName is required")]
        public string ResourceName { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
        public Guid ResourceTypeId { get; set; }
    }
}
