using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using AutoMapper;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
    /// <summary>
    /// Api controller.
    /// </summary>
    [Route("api")]

    public class ApiController : Controller
    {
        public ApiController()
        {
        }

        [HttpGet]
        public IActionResult Api()
        {
            return new JsonResult(new { success = true });
        }
    }
}
