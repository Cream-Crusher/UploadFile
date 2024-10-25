using GeneratePhotoService.Persistence.Database;
using GeneratePhotoService.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GeneratePhotoService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpClient();
            builder.Services.AddTransient<IPhotoRepository, PhotoRepository>();

            builder.Services.AddDbContext<ZelbikeChronoContext>(options => 
                options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(builder.Configuration.GetConnectionString("ZelbikeChronoContext"))
            );
            builder.Services.AddTransient<RabbitMqListener>();
            var app = builder.Build();
            
            app.Run();
        }
    }
}