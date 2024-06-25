using PrintManagement.Application.Payloads.RequestModels.InputRequests;
using PrintManagement.Application.Payloads.RequestModels.UserRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataUser;
using PrintManagement.Application.Payloads.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.InterfaceServices
{
    public interface IUserService
    {
        Task<IQueryable<DataResponseUser>> GetAllUsers(Request_InputUser request);
        Task<DataResponseUser> GetUserById(Guid id);
        Task<ResponseObject<DataResponseUser>> UpdateUser(Guid id, Request_UpdateUser request);
        Task<IQueryable<DataResponseUser>> GetAllUserContainsLeaderRole();
        Task<IQueryable<DataResponseUser>> GetAllUserContainsManagerRole();
    }
}
