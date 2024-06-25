using PrintManagement.Application.Payloads.RequestModels.KPIRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataKPI;
using PrintManagement.Application.Payloads.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.InterfaceServices
{
    public interface IKPIService
    {
        Task<ResponseObject<DataResponseKPI>> CreateKPIForEmployee(Request_CreateKPI request);
        Task<ResponseObject<DataResponseKPI>> UpdateKPIForEmployee(Request_UpdateKPI request);
        Task<ResponseObject<DataResponseKPI>> NotificationDoneKpi(Guid kpiId);
        Task<IQueryable<DataResponseKPI>> GetAllKpi(Guid? userId);
        Task<DataResponseKPI> GetKpiById(Guid kpiId);
    }
}
