using PrintManagement.Application.Payloads.ResponseModels.DataUser;
using PrintManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.ResponseModels.DataTeam
{
    public class DataResponseTeam : DataResponseBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? NumberOfMember { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string ManagerName { get; set; }
        public IQueryable<DataResponseUser> Users { get; set; }
    }
}
