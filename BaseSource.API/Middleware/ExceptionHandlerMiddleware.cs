using BaseSource.Application.Exceptions;
using BaseSource.Application.Wrappers;
using BaseSource.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using BadRequestException = BaseSource.Application.Exceptions.BadRequestException;
using NotFoundException = BaseSource.Application.Exceptions.NotFoundException;

namespace BaseSource.API.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await ConvertException(context, e, _logger);
            }
        }

        private Task ConvertException(HttpContext context, Exception exception, ILogger<ExceptionHandlerMiddleware> logger)
        {
            var httpStatusCode = HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var result = string.Empty;

            switch (exception)
            {
                case ValidationException _:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    break;
                case BadRequestException _:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    break;
                case NotFoundException _:
                    httpStatusCode = HttpStatusCode.NotFound;
                    break;
                case UnAuthorizeException _:
                    httpStatusCode = HttpStatusCode.Unauthorized;
                    break;
                case UnauthorizedAccessException _:
                    httpStatusCode = HttpStatusCode.NotAcceptable;
                    break;
                case ApiException _:
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    break;
                case DbUpdateException _:
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    result = JsonConvert.SerializeObject(new { message = new List<Message> { new Message(httpStatusCode.ToString(), MessagesKey.Exception_Database_Effect, MessageType.Error) } });
                    break;
                default:
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    result = JsonConvert.SerializeObject(new { message = new List<Message> { new Message(httpStatusCode.ToString(), exception.Message, MessageType.Error) } });
                    break;
            }

            logger.LogError(result);
            context.Response.StatusCode = (int)httpStatusCode;
            if (result == string.Empty)
            {
                result = JsonConvert.SerializeObject(new { message = exception.Data["Message"] });
            }

            return context.Response.WriteAsync(result);
        }
    }
}
