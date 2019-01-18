using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WiMD.IdentityAccess.Infrastructure
{
    public class JwtTokenProvider : IJwtTokenProvider
    {
        ISecretKeyProvider _secretKeyProvider;

        public JwtTokenProvider(ISecretKeyProvider secretKeyProvider)
        {
            _secretKeyProvider = secretKeyProvider;
        }

        public string CreateJwtToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKeyProvider.GetSecretKey()), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
