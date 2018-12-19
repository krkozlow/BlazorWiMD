using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.Authentication
{
    public interface IUserFactory
    {
        User CreateUser(string email, string password);
    }
}
