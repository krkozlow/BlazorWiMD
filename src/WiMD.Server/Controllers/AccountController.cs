using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WiMD.Authentication;

namespace WiMD.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost("[Action]")]
        public ActionResult<User> LogIn([FromBody]User model)
        {
            try
            {
                var user = _accountService.LogIn(model.Email, model.Password);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"User failed LogIn {model.Email}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[Action]")]
        public ActionResult<User> SignIn([FromBody]User model)
        {
            try
            {
                var user = _accountService.SignIn(model);

                return _accountService.LogIn(model.Email, model.Password);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"User failed SignIn {model.Email}");
                return BadRequest(ex.Message);
            }
        }
    }
}