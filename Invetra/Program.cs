using Inventra.Data;
using Inventra.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Inventra;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 1. Add services to the container (MVC)
        builder.Services.AddControllersWithViews();

        // 2. Add Razor Pages (Required for the pre-compiled Identity UI)
        builder.Services.AddRazorPages();

        // 3. Setup the Database Connection
        builder.Services.AddDbContext<InventraDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // 4. Configure Identity (The "Default UI" Trick)
        builder.Services.AddDefaultIdentity<InventraUser>(options => {
            options.SignIn.RequireConfirmedAccount = false; // Set to true if you want email confirmation later
            options.Password.RequireDigit = false;          // Making it easier for you to test
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<InventraDbContext>();

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        // 5. THE MIDDLEWARE ORDER IS CRITICAL:
        // Authentication must come AFTER Routing and BEFORE Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // 6. Map your routes
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // This maps the Identity "Account/Login" etc. routes automatically
        app.MapRazorPages();

        app.Run();
    }
}
