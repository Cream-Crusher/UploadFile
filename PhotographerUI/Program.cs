using LimetimePhotoUploadUI.Models.Repositories;
using LimetimePhotoUploadUI.Models.ZelbikeChrono;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LimetimePhotoUploadUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<ISelectelFileRepository, SelectelFileRepository>(opt => new SelectelFileRepository(builder.Configuration["Selectel:ApiEndpoint"], builder.Configuration["Selectel:AccessKey"], builder.Configuration["Selectel:SecretKey"]));
            builder.Services.AddDbContext<ZelbikeChronoContext>(options => 
                    options
                        .UseLazyLoadingProxies()
                        .UseSqlServer(builder.Configuration.GetConnectionString("ZelbikeChronoContext"))
            );
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{accessKey?}/{action=Index}/{photoGuid?}", new {Controller = "Home", action = "Index"});

            app.Run();
        }
    }
}