using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RestaurantOrders.Domain.Exceptions;

namespace RestaurantOrders.Api.Filters
{
    public class ValidationExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is DomainValidationException domainEx)
            {
                context.Result = new BadRequestObjectResult(new { error = domainEx.Message });
                context.ExceptionHandled = true;
            }
            else if (context.Exception is ArgumentException argEx)
            {
                context.Result = new BadRequestObjectResult(new { error = argEx.Message });
                context.ExceptionHandled = true;
            }
            else if (context.Exception is InvalidOperationException opEx)
            {
                context.Result = new BadRequestObjectResult(new { error = opEx.Message });
                context.ExceptionHandled = true;
            }
        }
    }
}
