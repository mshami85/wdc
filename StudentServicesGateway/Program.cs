using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace WDC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("Ocelot.json", optional: false, reloadOnChange: true);
            //builder.WebHost.UseUrls("http://localhost:5001");
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddOcelot();


            var allowCorsOrigins = "_allowAll";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: allowCorsOrigins, policy => policy.AllowAnyHeader().AllowCredentials().AllowAnyMethod().SetIsOriginAllowed(orig => true));
            });

            var app = builder.Build();
            app.MapControllers();
            app.UseRouting();
            app.UseCors(allowCorsOrigins);

            app.UseAuthorization();
            app.UseOcelot().Wait();

            app.Run();
        }
    }
}