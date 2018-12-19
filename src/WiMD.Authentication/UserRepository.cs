using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WiMD.Authentication
{
    public class UserRepository : IUserRepository
    {
        public User Create(User user)
        {
            users.Add(user);

            return user;
        }

        public User Get(string email)
        {
            return users.FirstOrDefault(x => x.Email == email);
        }

        static IList<User> users = new List<User>
        {
            new UserFactory(new JwtTokenProvider(new SecretKeyProvider(null))).CreateUser("andrzej", "golota", "andrew@email.com", "somePass"),
            new UserFactory(new JwtTokenProvider(new SecretKeyProvider(null))).CreateUser("mike", "tyson", "mike@email.com", "noPass"),
            new UserFactory(new JwtTokenProvider(new SecretKeyProvider(null))).CreateUser("lenox", "lewis", "lenox@email.com", "pass")
        };
    }
}