using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.Authentication
{
    public interface IAccountService
    {
        User SignIn(User user);
        User LogIn(string email, string password);
    }
}