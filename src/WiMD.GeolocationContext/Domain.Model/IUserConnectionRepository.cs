using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.GeolocationContext.Domain.Model
{
    public interface IUserConnectionRepository
    {
        UserConnection Get(long id);
        UserConnection GetByUserId(long userId);
        UserConnection Get(string connectionId);
        UserConnection Create(UserConnection user);
        UserConnection Update(UserConnection user);
        void Delete(UserConnection user);
        int ListenForUser(UserConnection user, UserConnection listenUser);
        IEnumerable<string> GetAllListeningUsers(UserConnection user);
    }
}

