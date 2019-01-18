using System;
using System.Collections.Generic;
using System.Text;
using WiMD.IdentityAccess.Infrastructure;

namespace WiMD.IdentityAccess.Domain.Model
{
    public class UserFactory : IUserFactory
    {
        public User CreateUser(string firstName, string lastName, string email, string password)
        {
            User user = new User();
            ValidateEmail(email);
            ValidatePassword(password);

            user.Email = email;
            user.Password = CryptographyHelper.HashPassword(password);
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Groups = new List<Group>();

            return user;
        }

        private void ValidatePassword(string password)
        {
            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be empty.");
            }
            int minPasswordLength = 3;
            if (password.Length < minPasswordLength)
            {
                throw new ArgumentException("Password has to be longer than 3 letters.");
            }
        }

        private void ValidateEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email cannot be empty.");
            }
            if (!email.Contains('@'))
            {
                throw new ArgumentException("Email has to contain @.");
            }
        }
    }
}
