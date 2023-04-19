using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace BaseSource.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiController<T> : ControllerBase
    {
        private IMediator _mediatorInstance;

        private ILogger<T> _loggerInstance;
        protected IMediator Mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
        protected ILogger<T> Logger => _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();

        public BaseApiController()
        {
            JsonSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
        protected JsonSerializerSettings JsonSettings { get; set; }
    }
}
