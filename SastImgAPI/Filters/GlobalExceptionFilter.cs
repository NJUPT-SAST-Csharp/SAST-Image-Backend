using Microsoft.AspNetCore.Mvc.Filters;
using Response.Builders;

namespace SastImgAPI.Filters
{
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            context.Result = ReponseBuilder
                .Error(StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
                .Add(context.Exception.GetType().Name, context.Exception.Message)
                .Build();
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
