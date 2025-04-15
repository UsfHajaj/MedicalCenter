using MedicalCenter.Services;

namespace MedicalCenter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllers();
            // Swagger services
            builder.Services.AddEndpointsApiExplorer(); // ãåã ÚáÔÇä Swagger íÔÊÛá
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerServices();
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddAuthenticationServices(builder.Configuration);


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(); // íÚÑÖ JSON ÈÊÇÚ Swagger
                app.UseSwaggerUI(); // æÇÌåÉ ÇáãÓÊÎÏã ÈÊÇÚÉ Swagger
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
