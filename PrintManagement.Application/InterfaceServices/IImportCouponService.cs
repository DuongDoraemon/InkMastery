using PrintManagement.Application.Payloads.RequestModels.ImportCouponRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataImportCoupon;
using PrintManagement.Application.Payloads.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.InterfaceServices
{
    public interface IImportCouponService
    {
        Task<ResponseObject<DataResponseImportCoupon>> CreateImportCoupon(Guid employeeId, Request_CreateImportCoupon request);
        Task<string> DeleteImportCoupon(Guid couponId);
        Task<IQueryable<DataResponseImportCoupon>> GetAllImporyCoupons();
        Task<DataResponseImportCoupon> GetImportCouponById(Guid couponId);
    }
}
