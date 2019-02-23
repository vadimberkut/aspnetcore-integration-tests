using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UserProfileManager.Entity.Dto;

namespace UserProfileManager.Filters
{
    /// <summary>
    /// Catches unhandled exceptions and forms an valid API error response
    /// </summary>
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment HostingEnvironemt;

        public CustomExceptionFilterAttribute(IHostingEnvironment hostingEnvironemt)
        {
            this.HostingEnvironemt = hostingEnvironemt;
        }

        public override void OnException(ExceptionContext context)
        {
            //if(this.HostingEnvironemt.IsDevelopment())
            //{
            //    return; // do nothing
            //}

            // TODO - exception checks for error messages will be implemented here
            // ...

            // To handle an exception, set the ExceptionContext.ExceptionHandled to true or write a response not both, because issue can appear
            var content = new JsonResponseDto
            {
                Data = null,
                Status = (int)HttpStatusCode.InternalServerError,
                Message = "Something went wrong during request processing" // user friendly error message goes here
            };
            context.Result = new ObjectResult(content)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
}
