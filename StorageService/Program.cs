using Amazon.S3;
using Minio;
using StorageService.Persistence.Repositories;

namespace StorageService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IFileRepository, MinioFileRepository>();  // or SelectelFileRepository
            builder.Services.AddControllers();
            // builder.Services.AddSwaggerGen();

            var app = builder.Build();
        
        if (app.Environment.IsDevelopment())  // swagger
        // {
            // app.UseSwagger();
            // app.UseSwaggerUI();
        // }
        
            app.UseHttpsRedirection();
            
            app.MapControllers();

            app.Run();
        }
    }
}