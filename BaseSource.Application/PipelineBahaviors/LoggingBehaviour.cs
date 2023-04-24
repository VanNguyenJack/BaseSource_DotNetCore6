using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Application.PipelineBahaviors
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Log.Information("Entering LoggingBehavior with request handling {Name}", typeof(TRequest).Name);

            Type myType = request.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(request, null);
                Log.Information("{Property} : {@Value}", prop.Name, propValue);
            }

            var response = await next();

            Log.Information("Leaving LoggingBehavior with request {Name}", typeof(TRequest).Name);

            return response;
        }
    }
}
