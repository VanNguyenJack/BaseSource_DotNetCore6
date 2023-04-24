using BaseSource.Domain;
using BaseSource.Domain.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace BaseSource.Application.Services.GlbAlert
{
    public class GlbAlertService : IGlbAlertService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;

        public GlbAlertService(IUnitOfWork unitOfWork, IDistributedCache distributedCache)
        {
            _unitOfWork = unitOfWork;
            _distributedCache = distributedCache;
        }

        public async Task<string> GetMessageAsync(string alertId)
        {
            return await Task.FromResult(alertId);
        }

        public async Task<string> GetMessageAsync(string alertId, params object[] parameters)
        {
            if (!string.IsNullOrEmpty(alertId))
            {
                return await Task.FromResult(string.Format(alertId, parameters));
            }
            return "Missing configuration message";
        }
    }
}
