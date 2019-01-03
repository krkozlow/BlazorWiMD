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

        public bool IsConnected { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<Group> Groups { get; set; }

        public void ValidateGivenPassword(string password)
        {
            if (CryptographyHelper.HashPassword(password) != Password)
            {
                throw new ArgumentException("Given password is not valid.");
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

        public void Connect()
        {
            IsConnected = true;
            //update repo
        }

        public void Disconnect()
        {
            IsConnected = false;
            //update repo
        }

        public void GenerateToken()
        {
            Token = _tokenProvider.CreateJwtToken(Email);
        }
    }
}