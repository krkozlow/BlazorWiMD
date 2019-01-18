using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WiMD.IdentityAccess.Infrastructure;

namespace WiMD.Server.Extensions
{
    public static class AuthenticationServiceExtension
    {
        const string accessTokenConst = "access_token";
        const string locationHubRouteConst = "/geolocationhub";

        public static void AddWiMDAuthentication(this IServiceCollection services, ISecretKeyProvider secretKeyProvider)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyProvider.GetSecretKey()),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query[accessTokenConst];

                        var path = context.HttpContext.Request.Path;
                        if (!String.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments(locationHubRouteConst))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
