using PrintManagement.Application.Payloads.ResponseModels.DataUser;
using PrintManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.ResponseModels.DataCustomerFeedback
{
    public class DataResponseCustomerFeedback : DataResponseBase
    {
        public string FeedbackContent { get; set; }
        public string ResponseByCompany { get; set; }
        public DataResponseUser UserFeedback { get; set; }
        public DateTime FeedbackTime { get; set; }
        public DateTime? ResponseTime { get; set; }
    }
}
