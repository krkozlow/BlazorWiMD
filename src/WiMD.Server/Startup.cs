using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using WiMD.Authentication;
using WiMD.Hub;

namespace WiMD.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            SecretKeyProvider = new SecretKeyProvider(configuration);

            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.RollingFile(Path.Combine(env.WebRootPath, "log-{Date}.log"))
                            .CreateLogger();
        }

        public IConfiguration Configuration { get; }
        public ISecretKeyProvider SecretKeyProvider { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ISecretKeyProvider, SecretKeyProvider>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
            services.AddScoped<IUserFactory, UserFactory>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddSingleton<IConnectionProvider, ConnectionProvider>();
            services.AddSingleton<IConnectionService, ConnectionService>();

            services.AddWiMDAuthentication(SecretKeyProvider);
            services.AddCors();
            services.AddSignalR();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "WiMD API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(options => 
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
                options.AllowCredentials();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            loggerFactory.AddSerilog();
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "WiMD API v1"); });
            app.UseSignalR(routes => routes.MapHub<LocationHub>("/locationhub"));
            app.UseMvc();
        }
    }
}