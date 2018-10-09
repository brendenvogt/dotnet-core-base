using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using AutoMapper;
using Swashbuckle.AspNetCore.Swagger;
using Infrastructure.Data.Repositories;
using Core.Entities;

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
        readonly IMongoRepository<User> _userRepo;

        /// <summary>
        /// Initializes a new instance of the controller
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="mapper">Mapper.</param>
        public UserController(IConfiguration configuration, ILogger logger, IMapper mapper, IMongoRepository<User> userRepo)
        {
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
            _userRepo = userRepo;
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
            _userRepo.AddAsync(new User
            {
                UserId = Guid.NewGuid()
            });

            return new JsonResult(new { success = true });
        }

        //name
        //phone number or email address

    }
}
