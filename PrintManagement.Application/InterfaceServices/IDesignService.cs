using PrintManagement.Application.Payloads.RequestModels.DesignRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataDesign;
using PrintManagement.Application.Payloads.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.InterfaceServices
{
    public interface IDesignService
    {
        Task<ResponseObject<DataResponseDesign>> CreateDesign(Guid designerId, Request_CreateDesign request);
        Task<ResponseObject<DataResponseDesign>> UpdateDesign(Guid designerId, Request_UpdateDesign request);
        Task<string> ApprovalDesign(Request_DesignApproval request);
    }
}
