using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.Authentication
{
    public interface IAuthenticationService
    {
        string Authenticate(User user);
    }
}