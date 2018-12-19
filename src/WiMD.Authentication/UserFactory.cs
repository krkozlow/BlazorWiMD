using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace WiMD.Authentication
{
    public class UserFactory : IUserFactory
    {
        private readonly IJwtTokenProvider _tokenProvider;
        private readonly IUserRepository _userRepository;

        public UserFactory(IJwtTokenProvider tokenProvider, IUserRepository userRepository)
        {
            _tokenProvider = tokenProvider;
            _userRepository = userRepository;
        }

        public User CreateUser(string email, string password)
        {
            User user = new User(_tokenProvider, _userRepository);
            ValidateEmail(email);
            ValidatePassword(password);

            var salt = GetSalt();
            var hashPassword = HashPassword(password, salt);

            user.Email = email;
            user.Password = hashPassword;

            return user;
        }

        //https://medium.com/@mehanix/lets-talk-security-salted-password-hashing-in-c-5460be5c3aae
        private string HashPassword(string password, byte[] salt)
        {
            int iterations = 1000;
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);

            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];

            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
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
