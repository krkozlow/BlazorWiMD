using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WiMD.Authentication;

namespace WiMD.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpGet("[Action]")]
        public ActionResult<IEnumerable<User>> GetConnected()
        {
            try
            {
                var users = _userRepository.GetConnectedUsers().ToArray();
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}