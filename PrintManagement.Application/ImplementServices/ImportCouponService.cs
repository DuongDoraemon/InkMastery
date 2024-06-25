using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using PrintManagement.Application.InterfaceServices;
using PrintManagement.Application.Payloads.Mappers;
using PrintManagement.Application.Payloads.RequestModels.ImportCouponRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataImportCoupon;
using PrintManagement.Application.Payloads.Responses;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.ImplementServices
{
    public class ImportCouponService : IImportCouponService
    {
        private readonly IBaseReposiroty<User> _baseUserRepository;
        private readonly IBaseReposiroty<ResourcePropertyDetail> _baseResourcePropertyDetailRepository;
        private readonly ImportCouponConverter _converter;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBaseReposiroty<ImportCoupon> _baseImportCouponRepository;
        private readonly IBaseReposiroty<ResourceProperty> _baseResourcePropertyRepository;
        private readonly IBaseReposiroty<Resource> _baseResourceRepository;
        
        public ImportCouponService(IBaseReposiroty<User> baseUserRepository, IBaseReposiroty<ResourcePropertyDetail> baseResourcePropertyDetailRepository, ImportCouponConverter converter, IHttpContextAccessor contextAccessor, IBaseReposiroty<ImportCoupon> baseImportCouponRepository, IBaseReposiroty<ResourceProperty> baseResourcePropertyRepository, IBaseReposiroty<Resource> baseResourceRepository)
        {
            _baseUserRepository = baseUserRepository;
            _baseResourcePropertyDetailRepository = baseResourcePropertyDetailRepository;
            _converter = converter;
            _contextAccessor = contextAccessor;
            _baseImportCouponRepository = baseImportCouponRepository;
            _baseResourcePropertyRepository = baseResourcePropertyRepository;
            _baseResourceRepository = baseResourceRepository;
        }

        public async Task<ResponseObject<DataResponseImportCoupon>> CreateImportCoupon(Guid employeeId, Request_CreateImportCoupon request)
        {
            var currentUser = _contextAccessor.HttpContext.User;
            try
            {
                if (!currentUser.Identity.IsAuthenticated)
                {
                    return new ResponseObject<DataResponseImportCoupon>
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Message = "Người dùng chưa được xác thực",
                        Data = null
                    };
                }
                if(!currentUser.IsInRole("Admin"))
                {
                    return new ResponseObject<DataResponseImportCoupon>
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Message = "Bạn không có quyền thực hiện chức năng này",
                        Data = null
                    };
                }
                var userImport = await _baseUserRepository.GetByIDAsync(employeeId);
                var resourcePropertyDetail = await _baseResourcePropertyDetailRepository.GetByIDAsync(request.ResourcePropertyDetailId);
                if(resourcePropertyDetail == null)
                {
                    return new ResponseObject<DataResponseImportCoupon>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy dữ liệu",
                        Data = null
                    };
                }
                var coupon = new ImportCoupon
                {
                    IsActive = true,
                    CreateTime = DateTime.Now,
                    EmployeeId = employeeId,
                    Quantity = request.Quantity,
                    ResourcePropertyDetailId = request.ResourcePropertyDetailId,
                    TotalMoney = resourcePropertyDetail.Price,
                    TradingCode = "InkMastery_" + new Random().Next(1000, 9999).ToString()
                };
                coupon = await _baseImportCouponRepository.CreateAsync(coupon);
                resourcePropertyDetail.Quantity += request.Quantity;
                await _baseResourcePropertyDetailRepository.UpdateAsync(resourcePropertyDetail);

                var resourceProperty = await _baseResourcePropertyRepository.GetAsync(x => x.Id == resourcePropertyDetail.ResourcePropertyId);
                resourceProperty.Quantity += request.Quantity;
                await _baseResourcePropertyRepository.UpdateAsync(resourceProperty);

                var resource = await _baseResourceRepository.GetAsync(x => x.Id == resourceProperty.ResourceId);
                resource.AvailableQuantity += request.Quantity;
                await _baseResourceRepository.UpdateAsync(resource);

                return new ResponseObject<DataResponseImportCoupon>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Thêm tài nguyên thành công",
                    Data = _converter.EntityToDTO(coupon)
                };
            }catch(Exception ex)
            {
                return new ResponseObject<DataResponseImportCoupon>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<string> DeleteImportCoupon(Guid couponId)
        {
            var currentUser = _contextAccessor.HttpContext.User;
            try
            {
                if (!currentUser.Identity.IsAuthenticated)
                {
                    return "Người dùng chưa xác thực";
                }
                if (!currentUser.IsInRole("Admin") && !currentUser.IsInRole("Leader"))
                {
                    return "Bạn không có quyền thực hiện chức năng này";
                }
                var coupon = await _baseImportCouponRepository.GetByIDAsync(couponId);
                if(coupon == null)
                {
                    return "Không tìm thấy phiếu nhập";
                }
                coupon.IsActive = false;
                await _baseImportCouponRepository.UpdateAsync(coupon);

                var resourcePropertyDetail = await _baseResourcePropertyDetailRepository.GetByIDAsync(coupon.ResourcePropertyDetailId);
                resourcePropertyDetail.Quantity -= coupon.Quantity;
                await _baseResourcePropertyDetailRepository.UpdateAsync(resourcePropertyDetail);

                var resourceProperty = await _baseResourcePropertyRepository.GetAsync(x => x.Id == resourcePropertyDetail.ResourcePropertyId);
                resourceProperty.Quantity -= coupon.Quantity;
                await _baseResourcePropertyRepository.UpdateAsync(resourceProperty);

                var resource = await _baseResourceRepository.GetAsync(x => x.Id == resourceProperty.ResourceId);
                resource.AvailableQuantity -= coupon.Quantity;
                await _baseResourceRepository.UpdateAsync(resource);

                return "Xóa thông tin phiếu nhập thành công";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public Task<IQueryable<DataResponseImportCoupon>> GetAllImporyCoupons()
        {
            throw new NotImplementedException();
        }

        public Task<DataResponseImportCoupon> GetImportCouponById(Guid couponId)
        {
            throw new NotImplementedException();
        }

    }
}
