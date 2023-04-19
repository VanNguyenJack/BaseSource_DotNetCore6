using Microsoft.AspNetCore.Http;
using BaseSource.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BaseSource.API.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            UserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(UserId))
            {
                UserId = "Unknown";
            }
            Claims = _httpContextAccessor.HttpContext?.User?.Claims.AsEnumerable().Select(item => new KeyValuePair<string, string>(item.Type, item.Value)).ToList();
        }

        public HttpRequest GetRequest()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            return request;
        }

        public string GetCurrentUserId()
        {
            return UserId;
        }

        public string GetCurrentUserEmail()
        {
            var email = _httpContextAccessor.HttpContext?.Request.Query["email"].ToString();

            return email;
        }
        public string GetCurrentCompanyId()
        {
            var companyId = _httpContextAccessor.HttpContext?.Request.Query["companyId"].ToString();

            return companyId;
        }

        public string GetCurrentIPAddress()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return httpContext.Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }
        public string GetSupervisorId()
        {
            return UserId;
        }

     

        private string UserId { get; }

        public List<KeyValuePair<string, string>> Claims { get; set; }

        public string GetEventId()
        {
            return _httpContextAccessor.HttpContext?.Request.Query["eventId"].ToString();
        }
    }
}
