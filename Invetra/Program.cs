using Inventra.Data;
using Inventra.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Inventra;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        
        builder.Services.AddControllersWithViews();

        builder.Services.AddRazorPages();

        builder.Services.AddDbContext<InventraDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("GigaByte")));

        builder.Services.AddDefaultIdentity<InventraUser>(options => {
            options.SignIn.RequireConfirmedAccount = false; // Set to true if you want email confirmation later
            options.Password.RequireDigit = false;          // Making it easier for you to test
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
            .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<InventraDbContext>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapRazorPages();

        using(var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Administratot", "ProductManager", "OrderManager" };

            foreach(var roleName in roleNames)
            {
                if(!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<InventraUser>>();

            // 1. Първо създаваме ролите (както досега)
            string[] roles = { "Administrator", "OrderManager", "InventoryManager" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role)); 
                }
            }

            // 2. Създаваме администраторския профил
            var adminEmail = "admin@inventra.com";
             var adminUser = await userManager.FindByEmailAsync(adminEmail); 

            if (adminUser == null)
            {
                var newAdmin = new InventraUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true // Маркираме го като потвърден
                };

                // Създаваме потребителя с парола по твой избор
                 var result = await userManager.CreateAsync(newAdmin, "Admin123!"); 

                if (result.Succeeded)
                {
                    // Присвояваме му ролята Administrator
                     await userManager.AddToRoleAsync(newAdmin, "Administrator"); 
                }
            }
        }

        app.Run();
    }
}
