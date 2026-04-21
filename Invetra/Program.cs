using Inventra.Data;
using Inventra.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Inventra.Core.Contracts;
using Inventra.Core.Services;
using Inventra.Controllers;
using Microsoft.IdentityModel.Tokens;

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
            options.SignIn.RequireConfirmedAccount = false; 
            options.Password.RequireDigit = false;          
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
            .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<InventraDbContext>();

        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<ICourierService, CourierService>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<ISupplierService, SupplierService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IOrderDetailsService, OrderDetailsService>();
        builder.Services.AddScoped<IMessageService, MessageService>();
        builder.Services.AddScoped<IDashboardService, DashboardService>();

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

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var dbContext = services.GetRequiredService<InventraDbContext>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<InventraUser>>();

                string[] roles = { "Administrator", "OrderManager", "InventoryManager" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                var adminEmail = "admin@inventra.com";
                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var newAdmin = new InventraUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(newAdmin, "Admin123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newAdmin, "Administrator");
                    }
                }

                var orderManagerEmail = "orders@inventra.com";
                if (await userManager.FindByEmailAsync(orderManagerEmail) == null)
                {
                    var newOrderManager = new InventraUser
                    {
                        UserName = orderManagerEmail,
                        Email = orderManagerEmail,
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(newOrderManager, "Orders123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newOrderManager, "OrderManager");
                    }
                }

                var inventoryManagerEmail = "inventory@inventra.com";
                if (await userManager.FindByEmailAsync(inventoryManagerEmail) == null)
                {
                    var newInventoryManager = new InventraUser
                    {
                        UserName = inventoryManagerEmail,
                        Email = inventoryManagerEmail,
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(newInventoryManager, "Inventory123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newInventoryManager, "InventoryManager");
                    }
                }

                InventraDbSeeder.Seed(dbContext);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred seeding the DB and Identity data.");
            }
        }

        app.Run();
    }
}
