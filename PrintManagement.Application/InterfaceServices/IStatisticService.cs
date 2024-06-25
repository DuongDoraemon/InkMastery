using PrintManagement.Application.Payloads.RequestModels.StatisticRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataStatistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.InterfaceServices
{
    public interface IStatisticService
    {
        Task<IQueryable<DataResponseStatisticSalesOfUser>> GetStatisticSalesOfUserAsync(Guid userId, Request_StatisticSalesOfUser request);
        Task<IQueryable<DataResponseStatisticSales>> GetStatisticSales(Request_StatisticSales request);
        Task<IQueryable<DataResponseStatisticSalary>> GetStatisticSalary(Guid userId);
    }
}
