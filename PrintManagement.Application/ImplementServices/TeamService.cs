using Microsoft.AspNetCore.Http;
using PrintManagement.Application.InterfaceServices;
using PrintManagement.Application.Payloads.Mappers;
using PrintManagement.Application.Payloads.RequestModels.TeamRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataTeam;
using PrintManagement.Application.Payloads.ResponseModels.DataUser;
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
    public class TeamService : ITeamService
    {
        private readonly IBaseReposiroty<User> _baseUserRepository;
        private readonly IBaseReposiroty<Team> _baseTeamRepository;
        private readonly TeamConverter _converter;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserRepository<User> _userRepository;
        private readonly IBaseReposiroty<Notification> _notificationRepository;
        private readonly UserConverter _userConverter;
        public TeamService(IBaseReposiroty<User> baseUserRepository, IBaseReposiroty<Team> baseTeamRepository, TeamConverter converter, IHttpContextAccessor contextAccessor, IUserRepository<User> userRepository, IBaseReposiroty<Notification> notificationRepository, UserConverter userConverter)
        {
            _baseUserRepository = baseUserRepository;
            _baseTeamRepository = baseTeamRepository;
            _converter = converter;
            _contextAccessor = contextAccessor;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
            _userConverter = userConverter;
        }

        public async Task<string> ChangeDepartmentForUser(Request_ChangeDepartmentForUser request)
        {
            var currentUser = _contextAccessor.HttpContext.User;
            try
            {
                if (!currentUser.Identity.IsAuthenticated)
                {
                    return "Người dùng chưa xác thực";
                }
                if (!currentUser.IsInRole("Admin"))
                {
                    return "Bạn không có quyền thực hiện chức năng này";
                }
                var employee = await _baseUserRepository.GetByIDAsync(request.EmployeeId);
                if(employee == null)
                {
                    return "Không tìm thấy nhân viên này";
                }
                if (_userRepository.GetRolesOfUserAsync(employee).Result.Contains("Manager"))
                {
                    return "Thực hiện không thành công! Người này đang giữ chức vụ trưởng phòng";
                }
                var team = await _baseTeamRepository.GetByIDAsync(request.TeamId);
                if(team == null)
                {
                    return "Không tìm thấy phòng ban";
                }
                employee.TeamId = request.TeamId;
                employee.UpdateTime = DateTime.Now;
                await _baseUserRepository.UpdateAsync(employee);
                team.NumberOfMember = await _baseUserRepository.CountAsync(x => x.TeamId == team.Id);
                await _baseTeamRepository.UpdateAsync(team);
                return "Thay đổi phòng ban cho người dùng thành công";
            }catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<ResponseObject<DataResponseTeam>> ChangeManagerForTeam(Request_ChangeManager request)
        {
            var currentUser = _contextAccessor.HttpContext.User;
            try
            {
                if (!currentUser.Identity.IsAuthenticated)
                {
                    return new ResponseObject<DataResponseTeam>
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Message = "Người dùng chưa xác thực",
                        Data = null
                    };
                }
                if (!currentUser.IsInRole("Admin"))
                {
                    return new ResponseObject<DataResponseTeam>
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Message = "Bạn không có quyền thực hiện chức năng này",
                        Data = null
                    };
                }
                var manager = await _baseUserRepository.GetByIDAsync(request.ManagerId);
                if (!_userRepository.GetRolesOfUserAsync(manager).Result.Contains("Manager"))
                {
                    return new ResponseObject<DataResponseTeam>
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Message = "Người này không có quyền quản lý",
                        Data = null
                    };
                }
                var team = await _baseTeamRepository.GetByIDAsync(request.TeamId);
                if(team == null)
                {
                    return new ResponseObject<DataResponseTeam>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy phòng ban",
                        Data = null
                    };
                }
                team.ManagerId = request.ManagerId;
                team.UpdateTime = DateTime.Now;
                await _baseTeamRepository.UpdateAsync(team);
                return new ResponseObject<DataResponseTeam>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Thay đổi trưởng phòng thành công",
                    Data = _converter.EntityToDTO(team)
                };
            }
            catch(Exception ex)
            {
                return new ResponseObject<DataResponseTeam>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<IQueryable<DataResponseUser>> GetAllUserByTeam(Guid teamId)
        {
            var query = await _baseUserRepository.GetAllAsync(item => item.IsActive == true && item.TeamId == teamId);
            return query.Select(item => _userConverter.EntityToDTOForUser(item));
        }

        public async Task<ResponseObject<DataResponseTeam>> CreateTeam(Request_CreateTeam request)
        {
            var currentUser = _contextAccessor.HttpContext.User;
            try
            {
                if (!currentUser.Identity.IsAuthenticated)
                {
                    return new ResponseObject<DataResponseTeam>
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Message = "Người dùng chưa xác thực",
                        Data = null
                    };
                }
                if (!currentUser.IsInRole("Admin"))
                {
                    return new ResponseObject<DataResponseTeam>
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Message = "Bạn không có quyền thực hiện chức năng này",
                        Data = null
                    };
                }
                var manager = await _baseUserRepository.GetByIDAsync(request.ManagerId);
                if(manager == null)
                {
                    return new ResponseObject<DataResponseTeam>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy người dùng",
                        Data = null
                    };
                }
                if (!_userRepository.GetRolesOfUserAsync(manager).Result.Contains("Manager"))
                {
                    return new ResponseObject<DataResponseTeam>
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Message = "Người này không có quyền manager",
                        Data = null
                    };
                }

                var team = new Team
                {
                    IsActive = true,
                    CreateTime = DateTime.Now,
                    Description = request.Description,
                    Id = Guid.NewGuid(),
                    ManagerId = request.ManagerId,
                    Name = request.Name,
                    NumberOfMember = 0
                };
                team = await _baseTeamRepository.CreateAsync(team);

                Notification notification = new Notification
                {
                    IsActive = true,
                    Content = $"Bạn đã được chuyển lên làm quản lý của phòng ban {team.Name}!",
                    Id = Guid.NewGuid(),
                    IsSeen = false,
                    Link = "",
                    UserId = manager.Id
                };
                notification = await _notificationRepository.CreateAsync(notification);

                return new ResponseObject<DataResponseTeam>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Tạo thông tin phòng ban thành công",
                    Data = _converter.EntityToDTO(team)
                };

            }catch(Exception ex)
            {
                return new ResponseObject<DataResponseTeam>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<string> DeleteTeam(Guid teamId)
        {
            var currentUser = _contextAccessor.HttpContext.User;
            try
            {
                if (!currentUser.Identity.IsAuthenticated)
                {
                    return "Người dùng chưa xác thực";
                }
                if (!currentUser.IsInRole("Admin"))
                {
                    return "Bạn không có quyền thực hiện chức năng này";
                }

                var team = await _baseTeamRepository.GetByIDAsync(teamId);
                if(team == null)
                {
                    return "Không tìm thấy phòng ban";
                }
                team.IsActive = false;
                team.UpdateTime = DateTime.Now;
                await _baseTeamRepository.UpdateAsync(team);
                return "Xóa thông tin phòng ban thành công";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<IQueryable<DataResponseTeam>> GetAllTeams(string? name)
        {
            var query = await _baseTeamRepository.GetAllAsync(x => x.IsActive == true);
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(record => record.Name.ToLower().Contains(name.ToLower()));
            }
            return query.Select(x => _converter.EntityToDTO(x));
        }

        public async Task<DataResponseTeam> GetTeamById(Guid teamId)
        {
            return _converter.EntityToDTO(await _baseTeamRepository.GetByIDAsync(teamId));
        }

        public async Task<ResponseObject<DataResponseTeam>> UpdateTeam(Request_UpdateTeam request)
        {
            var currentUser = _contextAccessor.HttpContext.User;
            try
            {
                if (!currentUser.Identity.IsAuthenticated)
                {
                    return new ResponseObject<DataResponseTeam>
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Message = "Người dùng chưa xác thực",
                        Data = null
                    };
                }
                
                var manager = await _baseUserRepository.GetByIDAsync(request.ManagerId);
                if (manager == null)
                {
                    return new ResponseObject<DataResponseTeam>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy người dùng",
                        Data = null
                    };
                }
                if (!_userRepository.GetRolesOfUserAsync(manager).Result.Contains("Manager"))
                {
                    return new ResponseObject<DataResponseTeam>
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Message = "Người dùng này không có quyền quản lý",
                        Data = null
                    };
                }
                var user = await _baseUserRepository.GetAsync(x => x.Id == Guid.Parse(currentUser.FindFirst("Id").Value));
                var team = await _baseTeamRepository.GetByIDAsync(request.Id);
                if(team == null)
                {
                    return new ResponseObject<DataResponseTeam>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy phòng ban",
                        Data = null
                    };
                }
                if (!currentUser.IsInRole("Admin") && !currentUser.IsInRole("Manager") && team.ManagerId != user.Id)
                {
                    return new ResponseObject<DataResponseTeam>
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Message = "Bạn không có quyền thực hiện chức năng này",
                        Data = null
                    };
                }
                team.UpdateTime = DateTime.Now;
                team.Description = request.Description;
                team.Name = request.Name;
                team.ManagerId = request.ManagerId;
                await _baseTeamRepository.UpdateAsync(team);
                return new ResponseObject<DataResponseTeam>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Cập nhật thông tin phòng ban thành công",
                    Data = _converter.EntityToDTO(team)
                };

            }
            catch (Exception ex)
            {
                return new ResponseObject<DataResponseTeam>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<IQueryable<DataResponseUser>> GetAllUserHaveRoleManager()
        {
            var listUser = await _baseUserRepository.GetAllAsync(record => record.IsActive == true);
            List<User> listResult = new List<User>();
            foreach(var user in listUser)
            {
                if (_userRepository.GetRolesOfUserAsync(user).Result.Contains("Manager"))
                {
                    listResult.Add(user);
                }
            }
            return listResult.Select(item => _userConverter.EntityToDTOForUser(item)).AsQueryable();
        }
    }
}
