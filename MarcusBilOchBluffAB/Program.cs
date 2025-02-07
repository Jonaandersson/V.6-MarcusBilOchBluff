using MarcusBilOchBluffAB.Data;
using MarcusBilOchBluffAB.Models;
using MarcusBilOchBluffAB.Repositories;
using MarcusBilOchBluffAB.Repositories.MarcusBilOchBluffAB.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MarcusBilOchBluffAB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IRepository<Car>, CarRepository>();
            builder.Services.AddScoped<IRepository<Booking>, BookingRepository>();
            builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();


            // sessionhantering
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Timeout för sessionen, 30 rimligt?
            });

            
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            
            app.UseRouting();
            app.UseSession(); // Aktivera sessionhantering

            app.UseAuthorization();

            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "admin",
                pattern: "admin/{action=Login}/{id?}",
                defaults: new { controller = "Admin", action = "Login" });

            app.Run();
        }
    }
}
