using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace AutoLotAPI_Core2.Filters
{
    public class AutoLotExceptionFilter : IExceptionFilter
    {
        private readonly Boolean is_development;
        public AutoLotExceptionFilter(IHostingEnvironment env)
        {
            this.is_development = env.IsDevelopment();
        }

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            String stack_trace = (this.is_development) ? ex.StackTrace : String.Empty;
            IActionResult action_result;
            String message = ex.Message;
            if (ex is DbUpdateConcurrencyException)
            {
                //Returns a 400
                if (!this.is_development)
                {
                    message = "There was an error updating the database. Another user has altered the record.";
                }
                action_result = new BadRequestObjectResult(
                    error: new { Error = "Concurrency Issue.", Message = ex.Message, StackTrace = stack_trace });
            }
            else
            {
                if (!this.is_development)
                {
                    message = "There was an unknown error. Please try again.";
                }
                action_result = new ObjectResult(
                    value: new { Error = "General Error.", Message = ex.Message, StackTrace = stack_trace })
                {
                    StatusCode = 500
                };
            }
            context.Result = action_result;
        }
    }
}
