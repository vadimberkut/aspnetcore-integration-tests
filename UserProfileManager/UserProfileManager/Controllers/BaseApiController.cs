using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserProfileManager.Entity.Dto;

namespace UserProfileManager.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class BaseApiController : Controller
    {
        protected IActionResult JsonResponse(object data, int status = 200, string message = null)
        {
            return Json(new JsonResponseDto
            {
                Data = data,
                Status = status,
                Message = message
            });
        }

        protected IActionResult EmptyJsonResponse(int status = 200, string message = null)
        {
            return this.JsonResponse(null, status, message);
        }
    }
}
