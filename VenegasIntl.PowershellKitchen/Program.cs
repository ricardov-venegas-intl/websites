using VenegasIntl.PowershellKitchen.Models;
using VenegasIntl.PowershellKitchen.Repositories;

namespace VenegasIntl.PowershellKitchen
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = new Configuration
            {
                PowershellBlogEntriesPath = builder.Configuration.GetValue<string>("powershellBlogEntriesPath")
            };

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddTransient<PowershellKitchenBlogRepository>();
			builder.Services.AddSingleton<Configuration>(configuration);

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}