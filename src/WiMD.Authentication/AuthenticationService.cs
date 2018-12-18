using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WiMD.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        ISecretKeyProvider _secretKeyProvider;

        private const string _email = "krzys@email.com";
        private const string _password = "haslo";

        public AuthenticationService(ISecretKeyProvider secretKeyProvider)
        {
            _secretKeyProvider = secretKeyProvider; 
        }

        public User Authenticate(User user)
        {
            if (!ValidatePassword(user))
            {
                throw new ArgumentException("Wrong password");
            }

            user.Token = CreateJwtToken(user);
            user.Password = null;

            return user;
        }

        private string CreateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKeyProvider.GetSecretKey()), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool ValidatePassword(User user)
        {
            return _email == user.Email && _password == user.Password;
        }
    }
}