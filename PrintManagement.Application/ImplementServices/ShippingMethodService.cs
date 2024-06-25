using Microsoft.AspNetCore.Http;
using PrintManagement.Application.InterfaceServices;
using PrintManagement.Application.Payloads.RequestModels.ShippingMethodRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataShippingMethod;
using PrintManagement.Application.Payloads.Responses;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using PrintManagement.Domain.InterfaceRepositories.InterfaceUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.ImplementServices
{
    public class ShippingMethodService : IShippingMethodService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBaseReposiroty<ShippingMethod> _baseReposiroty;
        private readonly IBaseReposiroty<User> _baseUserRepository;
        private readonly IUserRepository<User> _userRepository;
        private readonly IBaseReposiroty<Team> _teamRepository;
        public ShippingMethodService(IHttpContextAccessor contextAccessor, IBaseReposiroty<ShippingMethod> baseReposiroty, IBaseReposiroty<User> baseUserRepository, IUserRepository<User> userRepository, IBaseReposiroty<Team> teamRepository)
        {
            _contextAccessor = contextAccessor;
            _baseReposiroty = baseReposiroty;
            _baseUserRepository = baseUserRepository;
            _userRepository = userRepository;
            _teamRepository = teamRepository;
        }

        public async Task<ResponseObject<DataResponseShippingMethod>> CreateShippingMethod(Request_CreateShippingMethod request)
        {
            var currentUser = _contextAccessor.HttpContext.User;
            try
            {
                if (!currentUser.Identity.IsAuthenticated)
                {
                    return new ResponseObject<DataResponseShippingMethod>
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Message = "Người dùng chưa được xác thực",
                        Data = null
                    };
                }
                var user = await _baseUserRepository.GetAsync(x => x.Id == Guid.Parse(currentUser.FindFirst("Id").Value));
                var team = await _teamRepository.GetAsync(x => x.Id == user.TeamId);
                if (!currentUser.IsInRole("Manager") || !team.Name.Equals("Delivery") || team.ManagerId != user.Id)
                {
                    return new ResponseObject<DataResponseShippingMethod>
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Message = "Bạn không có quyền thực hiện chức năng này",
                        Data = null
                    };
                }
                ShippingMethod shippingMethod = new ShippingMethod
                {
                    IsActive = true,
                    Id = Guid.NewGuid(),
                    ShippingMethodName = request.ShippingMethodName,
                };
                shippingMethod = await _baseReposiroty.CreateAsync(shippingMethod);
                return new ResponseObject<DataResponseShippingMethod>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Thêm thông tin giao hàng thành công",
                    Data = null
                };
            }catch (Exception ex)
            {
                return new ResponseObject<DataResponseShippingMethod>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Data = null
                };
            }
        }
    }
}
