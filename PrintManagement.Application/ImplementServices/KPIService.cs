using Microsoft.AspNetCore.Http;
using PrintManagement.Application.InterfaceServices;
using PrintManagement.Application.Payloads.Mappers;
using PrintManagement.Application.Payloads.RequestModels.KPIRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataKPI;
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
    public class KPIService : IKPIService
    {
        private readonly IBaseReposiroty<Team> _baseTeamRepository;
        private readonly IUserRepository<User> _userRepository;
        private readonly IBaseReposiroty<User> _baseUserRepository;
        private readonly IBaseReposiroty<KeyPerformanceIndicators> _keyPerformanceIndicatorRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBaseReposiroty<Notification> _notificationRepository;
        private readonly KPIConverter _converter;
        public KPIService(IBaseReposiroty<Team> baseTeamRepository, IUserRepository<User> userRepository, IBaseReposiroty<KeyPerformanceIndicators> keyPerformanceIndicatorRepository, IHttpContextAccessor contextAccessor, IBaseReposiroty<User> baseUserRepository, IBaseReposiroty<Notification> notificationRepository, KPIConverter converter)
        {
            _baseTeamRepository = baseTeamRepository;
            _userRepository = userRepository;
            _keyPerformanceIndicatorRepository = keyPerformanceIndicatorRepository;
            _contextAccessor = contextAccessor;
            _baseUserRepository = baseUserRepository;
            _notificationRepository = notificationRepository;
            _converter = converter;
        }

        public async Task<ResponseObject<DataResponseKPI>> CreateKPIForEmployee(Request_CreateKPI request)
        {
            var currentUser = _contextAccessor.HttpContext.User;
            try
            {
                if (!currentUser.Identity.IsAuthenticated)
                {
                    return new ResponseObject<DataResponseKPI>
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Message = "Người dùng này chưa được xác thực",
                        Data = null
                    };
                }
                var user = await _baseUserRepository.GetAsync(x => x.Id == Guid.Parse(currentUser.FindFirst("Id").Value));
                var team = await _baseTeamRepository.GetAsync(x => x.Id == user.TeamId);
                if (team == null)
                {
                    return new ResponseObject<DataResponseKPI>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Phòng ban không tồn tại",
                        Data = null
                    };
                }
                if (!((currentUser.IsInRole("Manager") && team.Name.Equals("Sales")) && team.ManagerId != user.Id))
                {
                    return new ResponseObject<DataResponseKPI>
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Message = "Bạn không có quyền thực hiện chức năng này",
                        Data = null
                    };
                }

                var employee = await _baseUserRepository.GetAsync(x => x.Id == request.EmployeeId);
                if (employee == null)
                {
                    return new ResponseObject<DataResponseKPI>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Thông tin nhân viên không tồn tại",
                        Data = null
                    };
                }
                var teamOfEmployee = await _baseTeamRepository.GetAsync(x => x.Id == employee.TeamId);
                if (!_userRepository.GetRolesOfUserAsync(employee).Result.Contains("Employee") || !teamOfEmployee.Name.Equals("Sales"))
                {
                    return new ResponseObject<DataResponseKPI>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "KPI chỉ áp dụng cho phòng ban kinh doanh",
                        Data = null
                    };
                }
                KeyPerformanceIndicators key = new KeyPerformanceIndicators
                {
                    AchieveKPI = false,
                    ActuallyAchieved = 0,
                    IsActive = true,
                    EmployeeId = request.EmployeeId,
                    Id = Guid.NewGuid(),
                    IndicatorName = request.IndicatorName,
                    Period = request.Period,
                    Target = request.Target,
                };

                key = await _keyPerformanceIndicatorRepository.CreateAsync(key);
                Notification notification = new Notification
                {
                    IsActive = true,
                    Content = $"Bạn đã được giao KPI bởi trưởng phòng! Vui lòng kiểm tra thông tin để hoàn thành KPI đúng tiến độ",
                    Id = Guid.NewGuid(),
                    IsSeen = false,
                    Link = "",
                    UserId = request.EmployeeId
                };
                notification = await _notificationRepository.CreateAsync(notification);
                return new ResponseObject<DataResponseKPI>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Tạo KPI cho nhân viên thành công",
                    Data = _converter.EntityToDTO(key)
                };
            }
            catch (Exception ex)
            {
                return new ResponseObject<DataResponseKPI>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<IQueryable<DataResponseKPI>> GetAllKpi(Guid? userId)
        {
            var query = await _keyPerformanceIndicatorRepository.GetAllAsync(item => item.EmployeeId == userId);
            return query.Select(item => _converter.EntityToDTO(item));
        }

        public async Task<DataResponseKPI> GetKpiById(Guid kpiId)
        {
            var query = await _keyPerformanceIndicatorRepository.GetAsync(item => item.Id == kpiId);
            return _converter.EntityToDTO(query);

        }

        public async Task<ResponseObject<DataResponseKPI>> NotificationDoneKpi(Guid kpiId)
        {
            var kpi = await _keyPerformanceIndicatorRepository.GetByIDAsync(kpiId);
            if (kpi == null)
            {
                return new ResponseObject<DataResponseKPI>
                {
                    Status = StatusCodes.Status404NotFound,
                    Message = "Khách hàng này chưa có KPI",
                    Data = null
                };
            }
            var employee = await _baseUserRepository.GetByIDAsync(kpi.EmployeeId);
            if (kpi.AchieveKPI == true || (kpi.ActuallyAchieved >= kpi.Target))
            {
                return new ResponseObject<DataResponseKPI>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = $"Nhân viên {employee.FullName} chưa hoàn thành KPI",
                    Data = null
                };
            }

            Notification notification = new Notification
            {
                IsActive = true,
                Content = "Chúc mừng bạn đã hoàn thành KPI trong tháng này!",
                Id = Guid.NewGuid(),
                IsSeen = false,
                Link = "",
                UserId = employee.Id
            };
            
            notification = await _notificationRepository.CreateAsync(notification);

            return new ResponseObject<DataResponseKPI>
            {
                Status = StatusCodes.Status200OK,
                Message = "Xác nhận hoàn thành KPI",
                Data = _converter.EntityToDTO(kpi)
            };
        }

        public Task<ResponseObject<DataResponseKPI>> UpdateKPIForEmployee(Request_UpdateKPI request)
        {
            throw new NotImplementedException();
        }
    }
}
