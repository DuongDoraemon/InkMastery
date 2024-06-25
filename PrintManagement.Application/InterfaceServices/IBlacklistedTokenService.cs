using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.InterfaceServices
{
    public interface IBlacklistedTokenService
    {
        void BlacklistToken(string token, TimeSpan expiration);
        bool IsTokenBlacklisted(string token);
    }
}
