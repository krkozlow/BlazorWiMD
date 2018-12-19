﻿using Microsoft.AspNetCore.Mvc;
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
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
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
                return BadRequest(ex.Message);
            }
        }
    }
}