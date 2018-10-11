using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using AutoMapper;

using Core.Contracts;
using Infrastructure.Services;
using Infrastructure.Interfaces;

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
        readonly IUserService _userService;
 /// <summary>
        /// Initializes a new instance of the controller
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="mapper">Mapper.</param>
        public UserController(IConfiguration configuration, ILogger logger, IMapper mapper, IUserService userService)
        {
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
        }

        /// <summary>
        /// Signup using an email, password, and confirmPassword.
        /// </summary>
        /// <returns>JWT Token.</returns>
        [HttpPost("signup")]
        public IActionResult PostSignup(SignupUserContract signup)
        {
            var user = _userService.AddUser(signup, out var authInfo);
            return new JsonResult(new { user, authInfo});
        }

        /// <summary>
        /// Login Username endpoint using an email and password.
        /// </summary>
        /// <returns>JWT Token.</returns>
        [HttpPost("login", Name = "PostLogin")]
        public async Task<IActionResult> PostLogin(LoginUserContract login)
        {
            var authInfo = await _userService.LoginUserEmailAsync(login);
            return new JsonResult(new { authInfo });
        }

        /// <summary>
        /// Get current Authed user
        /// </summary>
        /// <returns>JWT Token.</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAuthedUser()
        {
            if (HttpContext.IsAuthed(out var userId)){
                return new JsonResult(await _userService.GetUserAsync(userId));
            }
            return BadRequest();
        }

    }
}
