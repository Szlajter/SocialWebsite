using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<User>(opt => {
                opt.Password.RequiredUniqueChars = 0;
            })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy =>policy.RequireRole("Admin"));
                options.AddPolicy("RequireAdminOrModeratorRole", policy =>policy.RequireRole("Admin", "Moderator"));
            });
            
            return services;
        }
    }
}