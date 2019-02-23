using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserProfileManager.Data.Repositories.Base;

namespace UserProfileManager.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class RolesController : BaseApiController
    {
        private readonly IHostingEnvironment HostingEnvironment = null;
        private readonly IUnitOfWork UnitOfWork = null;

        public RolesController(
            IHostingEnvironment hostingEnvironment,
            IUnitOfWork unitOfWork
        )
        {
            this.HostingEnvironment = hostingEnvironment;
            this.UnitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = this.UnitOfWork.UserProfileRepository.GetAllRoles();
            return this.JsonResponse(result);
        }
    }
}