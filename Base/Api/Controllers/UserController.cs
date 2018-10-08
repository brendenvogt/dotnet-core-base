using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using AutoMapper;
using Swashbuckle.AspNetCore.Swagger;

namespace Api.Controllers
{
    /// <summary>
    /// User controller.
    /// </summary>
    [Route("api/user")]
    public class UserController : Controller
    {
        readonly ILogger _logger;
        readonly IConfiguration _configuration;
        readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the controller
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="mapper">Mapper.</param>
        public UserController(IConfiguration configuration, ILogger logger, IMapper mapper)
        {
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Signup using an email, password, and confirmPassword.
        /// </summary>
        /// <returns>JWT Token.</returns>
        [HttpPost("signup")]
        public IActionResult PostSignup()
        {
            return new JsonResult(new { success = true });
        }

        /// <summary>
        /// Login Username endpoint using an email and password.
        /// </summary>
        /// <returns>JWT Token.</returns>
        [HttpPost("login", Name = "PostLogin")]
        public IActionResult PostLogin()
        {
            return new JsonResult(new { success = true });
        }

        //name
        //phone number or email address

    }
}
