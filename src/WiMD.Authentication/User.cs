using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace WiMD.Authentication
{
    public class User
    {
        private readonly IJwtTokenProvider _tokenProvider;

        public User(IJwtTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<Group> Groups { get; set; }

        public void ValidateGivenPassword(string password)
        {
            var hashPassword = Convert.FromBase64String(Password);
            int bytesOfSalt = 16;
            byte[] salt = new byte[bytesOfSalt];
            Array.Copy(hashPassword, 0, salt, 0, bytesOfSalt);

            int iterations = 10000;
            var pbkdfForGivenPassword = new Rfc2898DeriveBytes(password, salt, iterations);
            int passwordLendth = 20;
            byte[] hash = pbkdfForGivenPassword.GetBytes(passwordLendth);

            for (int i = 0; i < passwordLendth; i++)
            {
                if (hashPassword[i + bytesOfSalt] != hash[i])
                {
                    throw new ArgumentException("Given password is wrong.");
                }
            }
        }

        public IReadOnlyList<string> GetPublicGroups()
        {
            return Groups.Where(x => x.IsPublic).Select(x => x.Name).ToList();
        }

        public void AddToGroup(string groupName)
        {
            Groups.Add(new Group { Name = groupName, IsPublic = true });
        }

        public void RemoveFromGroup(string groupName)
        {
            var toRemove = Groups.FirstOrDefault(x => x.Name == groupName);

            if (toRemove == null)
            {
                throw new ArgumentException($"User does not belong to group {groupName}");
            }

            Groups.Remove(toRemove);
        }

        public void GenerateToken()
        {
            Token = _tokenProvider.CreateJwtToken(Email);
        }
    }
}