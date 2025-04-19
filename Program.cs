using MedicalCenter.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace MedicalCenter
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllers();
            // Swagger services
            builder.Services.AddEndpointsApiExplorer(); // „Â„ ⁄·‘«‰ Swagger Ì‘ €·
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerServices();
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddAuthenticationServices(builder.Configuration);


            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await roleManager.EnsureRolesCreatedAsync();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(); // Ì⁄—÷ JSON » «⁄ Swagger
                app.UseSwaggerUI(); // Ê«ÃÂ… «·„” Œœ„ » «⁄… Swagger
            }

            app.MapGet("/", context =>
            {
                context.Response.Redirect("/swagger");
                return Task.CompletedTask;
            });

            //using (var scope = app.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //     SeedRoles.SeedRolesAsync(services); 
            //}

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
