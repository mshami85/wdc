using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudentAdmission.Classes;
using StudentAdmission.Data;

namespace StudentAdmission
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                 .AddJsonFile($"appsettings.{env}.json", true, true);

            builder.Services.Configure<AppSettings>(builder.Configuration);

            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            builder.Services.AddDbContext<DataContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            var app = builder.Build();

            //initilize database
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<DataContext>();
                if (context == null)
                {
                    return;
                }
                context.Database.EnsureCreatedAsync().Wait();
                context.Database.MigrateAsync().Wait();
            }
            // Configure the HTTP request pipeline.
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}