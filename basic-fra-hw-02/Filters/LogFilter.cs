﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using basic_fra_hw_02.Exceptions;
using System.Net;

namespace basic_fra_hw_02.Filters
{
    public class LogFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"Sent response at: {DateTime.UtcNow.ToLongTimeString()}");

            if (context.Exception != null && !context.ExceptionHandled)
            {
                Console.WriteLine($"ERROR: {context.Exception.Message}");
                context.ExceptionHandled = true;

                string errorMessage;
                int statusCode;

                if (context.Exception.GetType() == typeof(UserErrorMessage))
                {
                    statusCode = (int)HttpStatusCode.BadRequest;
                    errorMessage = context.Exception.Message;
                }
                else
                {
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    errorMessage = "Web API encountered an error!";
                }

                context.Result = new ContentResult
                {
                    StatusCode = statusCode,
                    ContentType = "application/text",
                    Content = errorMessage,
                };
            }

            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"Got one request at: {DateTime.UtcNow.ToLongTimeString()}");
            base.OnActionExecuting(context);
        }
    }
}
