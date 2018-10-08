using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Api controller.
    /// </summary>
    [Route("api")]

    public class ApiController : Controller
    {
        [HttpGet]
        public IActionResult Api()
        {
            return new JsonResult(new { success = true });
        }
    }
}
