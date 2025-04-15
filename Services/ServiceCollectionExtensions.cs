using MedicalCenter.Model;
using MedicalCenter.Services.impelementation;
using MedicalCenter.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Runtime.CompilerServices;
using System.Text;

namespace MedicalCenter.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services ,IConfiguration configuration)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

        }


       
        public static void AddAuthenticationServices(this IServiceCollection services ,IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme=CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme= JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters 
                {
                    ValidateIssuer=true,
                    ValidIssuer = configuration["JWT:IssuerIp"],
                    ValidateAudience=true,
                    ValidAudience = configuration["JWT:AudienceIP"],
                    ValidateLifetime=true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecritKey"]))

                };

            }).AddCookie();

            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
        }


        public static void AddSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Medical Center",
                    Description = "ASP.NET Core Web API"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        public static async Task EnsureRolesCreatedAsync(this RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "admin", "user", "doctor" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
