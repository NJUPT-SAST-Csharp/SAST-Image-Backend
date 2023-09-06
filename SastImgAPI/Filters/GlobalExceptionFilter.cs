using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Response;
using SastImgAPI.Models.RequestDtos;

namespace SastImgAPI.Filters
{
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            context.Result = ResponseDispatcher
                .Error(StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
                .Add(context.Exception.GetType().Name, context.Exception.Message)
                .Build();
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
