using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels.ShippingMethodRequests
{
    public class Request_CreateShippingMethod
    {
        [Required(ErrorMessage = "ShippingMethodName is required")]
        public string ShippingMethodName { get; set; }
    }
}
