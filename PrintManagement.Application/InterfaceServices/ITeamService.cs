using PrintManagement.Application.Payloads.RequestModels.TeamRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataTeam;
using PrintManagement.Application.Payloads.ResponseModels.DataUser;
using PrintManagement.Application.Payloads.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.InterfaceServices
{
    public interface ITeamService
    {
        Task<ResponseObject<DataResponseTeam>> CreateTeam(Request_CreateTeam request);
        Task<ResponseObject<DataResponseTeam>> UpdateTeam(Request_UpdateTeam request);
        Task<string> DeleteTeam(Guid teamId);
        Task<IQueryable<DataResponseTeam>> GetAllTeams(string? name);
        Task<DataResponseTeam> GetTeamById(Guid teamId);
        Task<ResponseObject<DataResponseTeam>> ChangeManagerForTeam(Request_ChangeManager request);
        Task<string> ChangeDepartmentForUser(Request_ChangeDepartmentForUser request);
        Task<IQueryable<DataResponseUser>> GetAllUserHaveRoleManager();
        Task<IQueryable<DataResponseUser>> GetAllUserByTeam(Guid teamId);
    }
}
