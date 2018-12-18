using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WiMD.Authentication;

namespace WiMD.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public ActionResult<string> Post([FromBody]User model)
        {
            try
            {
                var token = _authenticationService.Authenticate(model);

                return token;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}