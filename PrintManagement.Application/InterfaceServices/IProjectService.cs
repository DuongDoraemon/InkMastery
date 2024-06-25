using PrintManagement.Application.Payloads.RequestModels.InputRequests;
using PrintManagement.Application.Payloads.RequestModels.ProjectRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataProject;
using PrintManagement.Application.Payloads.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.InterfaceServices
{
    public interface IProjectService
    {
        Task<ResponseObject<DataResponseProject>> CreateProject(Request_CreateProject request);
        Task<ResponseObject<DataResponseProject>> UpdateProject(Request_UpdateProject request);
        Task<string> DeleteProject(Guid projectId);
        Task<IQueryable<DataResponseProject>> GetAllProject(Request_InputProject? request);
        Task<DataResponseProject> GetProjectById(Guid projectId);
    }
}
