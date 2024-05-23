using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Movies.Application.Mediators.Validation;
using Movies.Domain.Validation;

namespace Movies.Infra.IoC
{
    public class HttpResponseExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception.Message.Contains("Entity could not be found"))
            {
                context.Result = new JsonResult("Entity could not be found");
                context.HttpContext.Response.StatusCode = 404;
                context.ExceptionHandled = true;
            }
            else if (context.Exception is DomainExceptionValidation)
            {
                context.Result = new JsonResult(context.Exception.Message);
                context.HttpContext.Response.StatusCode = 400;
                context.ExceptionHandled = true;
            }
            else if (context.Exception is BusinessExceptionValidation)
            {
                context.Result = new JsonResult(context.Exception.Message);
                context.HttpContext.Response.StatusCode = 400;
                context.ExceptionHandled = true;
            }
            else if (context.Exception is RpcException ex)
            {
                switch (ex.StatusCode)
                {
                    case StatusCode.NotFound:
                        context.Result = new JsonResult("External data could not be found");
                        context.HttpContext.Response.StatusCode = 404;
                        context.ExceptionHandled = true;
                        break;
                    default:
                        context.Result = new JsonResult(ex.Message);
                        context.HttpContext.Response.StatusCode = 500;
                        context.ExceptionHandled = true;
                        break;
                }
            }
        }
    }
}
