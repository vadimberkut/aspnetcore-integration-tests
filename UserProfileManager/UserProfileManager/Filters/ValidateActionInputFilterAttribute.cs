using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UserProfileManager.Entity.Dto;

namespace UserProfileManager.Filters
{
    /// <summary>
    /// Performs validation of action input model if it exist
    /// </summary>
    public class ValidateActionInputFilterAttribute : ActionFilterAttribute
    {
        public ValidateActionInputFilterAttribute ()
        {

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.ModelState.IsValid)
            {
                // Build model validation error object from model state
                Dictionary<string, IEnumerable<string>> modelErrors = context.ModelState.Select(x => new { Field = x.Key, Messages = x.Value.Errors.Select(y => y.ErrorMessage) }).ToDictionary(x => x.Field, y => y.Messages);

                var content = new JsonResponseDto
                {
                    Data = modelErrors,
                    Status = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid data was submited. Check your input and try again" // user friendly error message goes here
                };
                context.Result = new ObjectResult(content) {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }
    }
}
