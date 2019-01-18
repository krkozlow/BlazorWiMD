using System;
using System.Collections.Generic;
using System.Text;
using WiMD.IdentityAccess.Domain.Model;

namespace WiMD.IdentityAccess.Application
{
    public interface IAccountService
    {
        User SignIn(User user);
        User LogIn(string email, string password);
    }
}
