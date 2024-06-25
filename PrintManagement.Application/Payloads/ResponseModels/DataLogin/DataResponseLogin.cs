using PrintManagement.Application.Payloads.ResponseModels.DataUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.Payloads.ResponseModels.DataLogin
{
    public class DataResponseLogin : DataResponseBase
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DataResponseUser User { get; set; }
    }
}
