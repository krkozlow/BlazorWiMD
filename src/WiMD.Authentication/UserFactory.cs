using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace WiMD.Authentication
{
    public class UserFactory : IUserFactory
    {
        private readonly IJwtTokenProvider _tokenProvider;

        public UserFactory(IJwtTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public User CreateUser(string firstName, string lastName, string email, string password)
        {
            User user = new User(_tokenProvider);
            ValidateEmail(email);
            ValidatePassword(password);

            user.Email = email;
            user.Password = CryptographyHelper.HashPassword(password);
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Groups = new List<Group>();

            return user;
        }

        private byte[] GetSalt()
        {
            byte[] salt;

            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            return salt;

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
