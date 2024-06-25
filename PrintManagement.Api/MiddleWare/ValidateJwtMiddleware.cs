using PrintManagement.Application.ImplementServices;
using PrintManagement.Application.InterfaceServices;

namespace PrintManagement.Api.MiddleWare
{
    public class ValidateJwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IBlacklistedTokenService _blacklistedTokenService;

        public ValidateJwtMiddleware(RequestDelegate next, IBlacklistedTokenService blacklistedTokenService)
        {
            _next = next;
            _blacklistedTokenService = blacklistedTokenService;
        }

        public async Task Invoke(HttpContext context)
        {
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token) && _blacklistedTokenService.IsTokenBlacklisted(token))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Token không hợp lệ.");
                return;
            }

            await _next(context);
        }
    }
}
