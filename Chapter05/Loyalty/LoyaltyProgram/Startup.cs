using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace LoyaltyProgram
{
    public class Startup
    {
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme
                )
                .AddJwtBearer(o => o.TokenValidationParameters = 
                new TokenValidationParameters
                {
                    // Do not use this configuration in production
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = false,
                    SignatureValidator = (token, parameter) => 
                    new JwtSecurityToken(token)
                });
            services.AddControllers();
        }

      
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /* Middleware lambda example
            app.Use(next => ctx =>
            {
                Console.WriteLine("Got request in lambda middleware");
                Console.WriteLine($"Request Path:{ctx.Request.Path}");
                return next(ctx);
            });
            */
            // app.Use(next => new ConsoleMiddleware(next).Invoke);
            // app.UseMiddleware<ConsoleMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
