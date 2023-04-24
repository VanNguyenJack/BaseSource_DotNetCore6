using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Domain.Services
{
    public interface IGlbAlertService
    {
        public Task<string> GetMessageAsync(string alertId);

        public Task<string> GetMessageAsync(string alertId, params object[] parameters);
    }
}
