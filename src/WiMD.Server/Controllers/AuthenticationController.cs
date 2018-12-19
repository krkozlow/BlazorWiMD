using Microsoft.AspNetCore.Mvc;
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
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AuthenticationController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public ActionResult<User> Post([FromBody]User model)
        {
            try
            {
                var user = _accountService.LogIn(model.Email, model.Password);
                return user;
            }
            catch (Exception ex)
            {
                model.Token = ex.Message;
                return model;
            }
        }
    }
}