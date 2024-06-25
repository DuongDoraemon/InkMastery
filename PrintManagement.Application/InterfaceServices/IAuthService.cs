using PrintManagement.Application.Payloads.RequestModels.UserRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataLogin;
using PrintManagement.Application.Payloads.ResponseModels.DataRole;
using PrintManagement.Application.Payloads.ResponseModels.DataUser;
using PrintManagement.Application.Payloads.Responses;
using PrintManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.InterfaceServices
{
    public interface IAuthService
    {
        Task<ResponseObject<DataResponseUser>> Register(Request_Register request);
        Task<ResponseObject<DataResponseLogin>> Login(Request_Login request);
        Task<ResponseObject<DataResponseLogin>> GetJwtTokenAsync(User user);
        Task<string> ChangePassword(Guid userId, Request_ChangePassword request);
        Task<string> ForgotPassword(string email);
        Task<string> ConfirmCreateNewPassword(Request_ConfirmCreateNewPassword request);
        Task<ResponseObject<DataResponseUser>> AddRoleToUser(Guid userId, List<string> roles);
        Task<IQueryable<DataResponseRole>> GetAllRoles();
        Task<IQueryable<string>> GetRolesByUserId(Guid userId);
    }
}
