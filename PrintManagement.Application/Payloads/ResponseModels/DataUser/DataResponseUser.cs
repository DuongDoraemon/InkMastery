using PrintManagement.Application.Payloads.ResponseModels.DataNotification;
using PrintManagement.Application.Payloads.ResponseModels.DataTeam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.ResponseModels.DataUser
{
    public class DataResponseUser : DataResponseBase
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public string TeamName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreateTime { get; set; }
        public IQueryable<DataResponseNotification>? DataResponseNotifications { get; set; }
    }
}
