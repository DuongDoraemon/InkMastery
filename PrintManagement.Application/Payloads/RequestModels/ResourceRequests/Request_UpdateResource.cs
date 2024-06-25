using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels.ResourceRequests
{
    public class Request_UpdateResource
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "ResourceName is required")]
        public string ResourceName { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}
