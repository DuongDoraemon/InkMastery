using Microsoft.AspNetCore.Http;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels.ProjectRequests
{
    public class Request_CreateProject
    {
        [Required(ErrorMessage = "ProjectName is required")]
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "RequestDescriptionFromCustomer is required")]
        public string RequestDescriptionFromCustomer { get; set; }
        [Required]
        public Guid LeaderId { get; set; }
        [Required]
        public DateTime ExpectedEndDate { get; set; }
        [Required]
        public Guid CustomerId { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal CommissionPercentage { get; set; }
        public IFormFile ImageDescription { get; set; }
    }
}
