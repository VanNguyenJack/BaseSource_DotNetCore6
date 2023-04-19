using Microsoft.AspNetCore.Http;

namespace BaseSource.Domain.Services
{
    public interface ISessionService
    {
        HttpRequest GetRequest();
        public string GetCurrentUserId();
        public string GetCurrentIPAddress();

        public string GetEventId();
        public string GetCurrentUserEmail();
    }
}
