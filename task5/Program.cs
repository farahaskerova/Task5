//using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
using Task.Contracts;
using Task.Database;
//using Task.Services;
//using Task.Services.Abstract;


namespace Task
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            builder.Services
                .AddDbContext<TaskDbContext>(o =>
                {
                    o.UseNpgsql(DatabaseConstants.CONNECTION_STRING);
                });

            var app = builder.Build();

            app.UseStaticFiles();

            app.MapControllerRoute("default", "{controller=Home}/{action=Index}");

            app.Run();
        }
    }

}