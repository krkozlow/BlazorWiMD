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

        private const string _userName = "krzys";
        private const string _password = "haslo";

        public AuthenticationService(ISecretKeyProvider secretKeyProvider)
        {
            _secretKeyProvider = secretKeyProvider; 
        }

        public string Authenticate(User user)
        {
            if (!ValidatePassword(user.Name, user.Password))
            {
                throw new ArgumentException("Wrong password");
            }

            return CreateJwtToken();
        }

        private string CreateJwtToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, _userName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKeyProvider.GetSecretKey()), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool ValidatePassword(string userName, string password)
        {
            return _userName == userName && _password == password;
        }
    }
}