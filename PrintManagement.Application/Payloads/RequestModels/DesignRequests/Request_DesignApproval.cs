using PrintManagement.Domain.Enumerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.RequestModels.DesignRequests
{
    public class Request_DesignApproval
    {
        public Guid DesignId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DesignApproval DesignApproval { get; set; }
    }
}
