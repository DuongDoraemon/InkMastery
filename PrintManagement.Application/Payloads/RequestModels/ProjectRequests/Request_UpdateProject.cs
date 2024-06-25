using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels.ProjectRequests
{
    public class Request_UpdateProject
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string RequestDescriptionFromCustomer { get; set; }
        public decimal StartingPrice { get; set; }
        public Guid LeaderId { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public Guid CustomerId { get; set; }
    }
}
