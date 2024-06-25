using Microsoft.AspNetCore.Http;
using PrintManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels.ResourceRequests
{
    public class Request_CreateResourcePropertyDetail
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
