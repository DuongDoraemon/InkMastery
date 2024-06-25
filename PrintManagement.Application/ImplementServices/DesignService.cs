using Microsoft.AspNetCore.Http;
using PrintManagement.Application.Handle.HandleFile;
using PrintManagement.Application.InterfaceServices;
using PrintManagement.Application.Payloads.Mappers;
using PrintManagement.Application.Payloads.RequestModels.DesignRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataDesign;
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
    public class DesignService : IDesignService
    {
        private readonly IBaseReposiroty<User> _baseUserRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseReposiroty<Design> _baseDesignRepository;
        private readonly IBaseReposiroty<Team> _baseTeamRepository;
        private readonly IBaseReposiroty<Project> _baseProjectReposiroty;
        private readonly DesignConverter _mapper;
        private readonly IUserRepository<User> _userRepository;
        private readonly IBaseReposiroty<Notification> _notificationRepository;
        public DesignService(IBaseReposiroty<User> baseUserRepository, IHttpContextAccessor httpContextAccessor, IBaseReposiroty<Design> baseDesignRepository, DesignConverter mapper, IBaseReposiroty<Project> baseProjectReposiroty, IBaseReposiroty<Team> baseTeamRepository, IUserRepository<User> userRepository, IBaseReposiroty<Notification> notificationRepository)
        {
            _baseUserRepository = baseUserRepository;
            _httpContextAccessor = httpContextAccessor;
            _baseDesignRepository = baseDesignRepository;
            _mapper = mapper;
            _baseProjectReposiroty = baseProjectReposiroty;
            _baseTeamRepository = baseTeamRepository;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task<string> ApprovalDesign(Request_DesignApproval request)
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            var leader = currentUser.FindFirst("Id").Value;
            try
            {
                if (!currentUser.Identity.IsAuthenticated)
                {
                    return "Người dùng chưa được xác thực";
                }
                if (!currentUser.IsInRole("Leader"))
                {
                    return "Bạn không có quyền thực hiện chức năng này";
                }
                var design = await _baseDesignRepository.GetByIDAsync(request.DesignId);
                if(design == null)
                {
                    return "Không tìm thấy thiết kế";
                }
                var project = await _baseProjectReposiroty.GetByIDAsync(design.ProjectId);
                if(Guid.Parse(leader) != project.LeaderId)
                {
                    return "Bạn không phải là leader của dự án này";
                }
                if (design.DesignStatus.ToString().Equals("HasBeenApproved"))
                {
                    return "Thiết kế này đã được duyệt trước đó";
                }
                var listDesign = await _baseDesignRepository.GetAllAsync(record => record.ProjectId == project.Id && record.IsActive == true && record.DesignerId == design.DesignerId && record.DesignStatus == Domain.Enumerates.DesignStatusEnum.HasBeenApproved);
                if(listDesign.ToList().Count >= 1)
                {
                    return "Đã có thiết kế được duyệt trước đó";
                }
                if (request.DesignApproval.ToString().Equals("Agree"))
                {
                    design.DesignStatus = Domain.Enumerates.DesignStatusEnum.HasBeenApproved;
                    design.ApproverId = Guid.Parse(currentUser.FindFirst("Id").Value);
                    await _baseDesignRepository.UpdateAsync(design);
                    project.ProjectStatus = Domain.Enumerates.ProjectStatusEnum.Approved;
                    project.Progress = 50;
                    await _baseProjectReposiroty.UpdateAsync(project);

                    Notification notification = new Notification
                    {
                        IsActive = true,
                        Content = "Thiết kế của bạn đã được duyệt",
                        Id = Guid.NewGuid(),
                        IsSeen = false,
                        Link = "",
                        UserId = design.DesignerId
                    };

                    notification = await _notificationRepository.CreateAsync(notification);
                    return "Đã duyệt thiết kế";
                }
                else
                {
                    design.DesignStatus = Domain.Enumerates.DesignStatusEnum.Refuse;
                    design.ApproverId = Guid.Parse(currentUser.FindFirst("Id").Value);
                    await _baseDesignRepository.UpdateAsync(design);
                    Notification notification = new Notification
                    {
                        IsActive = true,
                        Content = "Thiết kế của bạn bị từ chối phê duyệt",
                        Id = Guid.NewGuid(),
                        IsSeen = false,
                        Link = "",
                        UserId = design.DesignerId
                    };

                    notification = await _notificationRepository.CreateAsync(notification);
                    return "Không duyệt thiết kế";
                }
            }
            catch(Exception ex)
            {
                return "Error: " + ex.Message; 
            }
        }

        public async Task<ResponseObject<DataResponseDesign>> CreateDesign(Guid designerId, Request_CreateDesign request)
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            try
            {
                if (!currentUser.Identity.IsAuthenticated)
                {
                    return new ResponseObject<DataResponseDesign>
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Message = "Người dùng chưa được xác thực",
                        Data = null
                    };
                }
                if (!currentUser.IsInRole("Designer"))
                {
                    return new ResponseObject<DataResponseDesign>
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Message = "Bạn không có quyền thực hiện chức năng này",
                        Data = null
                    };
                }
                var designer = await _baseUserRepository.GetByIDAsync(designerId);
                var team = await _baseTeamRepository.GetAsync(x => x.Id == designer.TeamId);
                if (!team.Name.Equals("Technical"))
                {
                    return new ResponseObject<DataResponseDesign>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Người dùng này không trong phòng ban kĩ thuật",
                        Data = null
                    };
                }
                var listDesign = await _baseDesignRepository.GetAllAsync(x => x.ProjectId == request.ProjectId && x.DesignStatus == Domain.Enumerates.DesignStatusEnum.HasBeenApproved);
                if(listDesign.ToList().Count > 0)
                {
                    return new ResponseObject<DataResponseDesign> 
                    { 
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Dự án này đã có thiết kế được phê duyệt",
                        Data = null
                    };
                }
                if (!_userRepository.GetRolesOfUserAsync(designer).Result.Contains("Designer"))
                {
                    return new ResponseObject<DataResponseDesign>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Người dùng không có quyền design",
                        Data = null
                    };
                }
                var project = await _baseProjectReposiroty.GetByIDAsync(request.ProjectId);
                if(project == null)
                {
                    return new ResponseObject<DataResponseDesign>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Thông tin dự án không tồn tại",
                        Data = null
                    };
                }
                
                project.ProjectStatus = Domain.Enumerates.ProjectStatusEnum.Designing;
                await _baseProjectReposiroty.UpdateAsync(project);
                Design design = new Design
                {
                    IsActive = true,
                    DesignerId = designerId,
                    DesignImage = await HandleUploadFile.Upfile(request.DesignImage),
                    DesignStatus = Domain.Enumerates.DesignStatusEnum.NotYetApproved,
                    DesignTime = DateTime.Now,
                    ProjectId = request.ProjectId,
                    Id = Guid.NewGuid()
                };
                design = await _baseDesignRepository.CreateAsync(design);
                project.ProjectStatus = Domain.Enumerates.ProjectStatusEnum.AwaitingApproval;
                project.Progress = 25;
                await _baseProjectReposiroty.UpdateAsync(project);
                return new ResponseObject<DataResponseDesign>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Tạo bản thiết kế thành công",
                    Data = _mapper.EntityToDTOForDesign(design)
                };
            }catch(Exception ex)
            {
                return new ResponseObject<DataResponseDesign>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ResponseObject<DataResponseDesign>> UpdateDesign(Guid designerId, Request_UpdateDesign request)
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            try
            {
                var design = await _baseDesignRepository.GetByIDAsync(request.DesignId);
                if (!currentUser.Identity.IsAuthenticated)
                {
                    return new ResponseObject<DataResponseDesign>
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Message = "UnAuthenticated user",
                        Data = null
                    };
                }
                if (!currentUser.IsInRole("Designer") || design.DesignerId != Guid.Parse(currentUser.FindFirst("Id").Value))
                {
                    return new ResponseObject<DataResponseDesign>
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Message = "You do not have permission to perform this function",
                        Data = null
                    };
                }
                var designer = await _baseUserRepository.GetByIDAsync(designerId);
                var project = await _baseProjectReposiroty.GetByIDAsync(request.ProjectId);
                if (project == null)
                {
                    return new ResponseObject<DataResponseDesign>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Project not found",
                        Data = null
                    };
                }
                
                if(design == null)
                {
                    return new ResponseObject<DataResponseDesign>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Design not found",
                        Data = null
                    };
                }
                if(design.DesignStatus.ToString().Equals("HasBeenApproved") || design.DesignStatus.ToString().Equals("Refuse"))
                {
                    return new ResponseObject<DataResponseDesign>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "This design has been approved or rejected so it cannot be edited",
                        Data = null
                    };
                }
                design.DesignImage = await HandleUploadFile.WriteFile(request.DesignImage);
                design.ProjectId = request.ProjectId;
                design = await _baseDesignRepository.UpdateAsync(design);
                return new ResponseObject<DataResponseDesign>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Design updated successfully",
                    Data = _mapper.EntityToDTOForDesign(design)
                };
            }
            catch (Exception ex)
            {
                return new ResponseObject<DataResponseDesign>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Data = null
                };
            }
        }
    }
}
