using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.IdentityAccess.Domain.Model
{
    public interface IUserFactory
    {
        User CreateUser(string firstName, string lastName, string email, string password);
    }
}
