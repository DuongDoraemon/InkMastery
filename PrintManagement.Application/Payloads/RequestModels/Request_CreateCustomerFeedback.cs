using PrintManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels
{
    public class Request_CreateCustomerFeedback
    {
        public Guid ProjectId { get; set; }
        public string FeedbackContent { get; set; }
        public DateTime FeedbackTime { get; set; }
    }
}
