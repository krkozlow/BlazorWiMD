﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.Authentication
{
    public interface IUserRepository
    {
        User Create(User user);
        User Get(string email);
        User Get(int id);
        User Update(User user);
        IEnumerable<User> GetConnectedUsers();
        IEnumerable<User> GetUsers();
    }
}
