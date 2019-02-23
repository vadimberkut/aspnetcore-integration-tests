using Microsoft.AspNetCore.Mvc;

namespace UserProfileManager.Controllers
{
    public class TestController : BaseApiController
    {
        public IActionResult Empty()
        {
            return base.EmptyJsonResponse();
        }

        public IActionResult Text()
        {
            return base.JsonResponse("test", 200);
        }
    }
}