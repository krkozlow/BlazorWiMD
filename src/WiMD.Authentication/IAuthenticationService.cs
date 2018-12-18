using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.Authentication
{
    public interface IAuthenticationService
    {
        User Authenticate(User user);
    }
}