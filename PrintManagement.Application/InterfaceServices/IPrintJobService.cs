using PrintManagement.Application.Payloads.RequestModels.PrintJobRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataPrintJob;
using PrintManagement.Application.Payloads.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.InterfaceServices
{
    public interface IPrintJobService
    {
        Task<ResponseObject<DataResponsePrintJob>> CreatePrintJob(Request_CreatePrintJob request);
        Task<ResponseObject<DataResponsePrintJob>> ConfirmDonePrintJob(Guid printJobId);
        Task<IQueryable<DataResponsePrintJob>> GetAllPrintJobs();
        Task<DataResponsePrintJob> GetPrintJobById(Guid printJobId);
    }
}
