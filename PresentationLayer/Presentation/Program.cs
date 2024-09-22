using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using TranslationNation.Web.Models;
using System.Net;  // <-- Added for ServicePointManager

internal class Program
{
    private static void Main(string[] args)
    {
        // Ensure that TLS 1.2 is enabled for secure connections
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddControllersWithViews();

        // Configure cookie-based authentication
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

        // Add other services like authorization and email service
        builder.Services.AddAuthorization();
        builder.Services.AddDataProtection();
        builder.Services.AddTransient<EmailService>();  // Register EmailService

        var app = builder.Build();

        // Configure middleware
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        // Authentication and authorization middleware
        app.UseAuthentication();
        app.UseAuthorization();

        // Configure endpoint routing
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Customer}/{action=Index}/{id?}");

        // Start the app
        app.Run();
    }
}
