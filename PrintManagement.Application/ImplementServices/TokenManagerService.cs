using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using PrintManagement.Application.InterfaceServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.ImplementServices
{
    public class TokenManagerService : ITokenManagerService
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenManagerService(IDistributedCache cache, IHttpContextAccessor httpContextAccessor) 
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task DeactivateAsync(string token) => await _cache.SetStringAsync(GetKey(token), " ", new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
        });

        public async Task DeactivateCurrentAsync()=> await DeactivateAsync(GetCurrentAsync());

        public async Task<bool> IsActiveAsync(string token)=> await _cache.GetStringAsync(GetKey(token)) == null;
        

        public async Task<bool> IsCurrentActiveToken()
        {
            return await IsActiveAsync(GetCurrentAsync());
        }
        private string GetCurrentAsync()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["authorization"];
            return authorizationHeader == StringValues.Empty ? string.Empty : authorizationHeader.Single().Split(" ").Last();
        }
        private static string GetKey(string token) => $"Token: {token} deactivated";
    }
}
