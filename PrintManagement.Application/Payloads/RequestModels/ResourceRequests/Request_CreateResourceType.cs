using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels.ResourceRequests
{
    public class Request_CreateResourceType
    {
        [Required(ErrorMessage = "NameOfResourceType is required")]
        public string NameOfResourceType { get; set; }
    }
}
