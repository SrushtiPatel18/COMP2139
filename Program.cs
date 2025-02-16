using Assignment1.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{ var context = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
    try
    {
        if (!context.Categories.Any()) 
        {
            context.Categories.AddRange(
                new Categories { Name = "Electronics" },
                new Categories { Name = "Books" },
                new Categories { Name = "Clothing" },
                new Categories { Name = "HomeAppliances" }
            );
            context.SaveChanges();
        }
    }
    catch (DbUpdateException dbEx)
    {
        Console.WriteLine($"Database update error: {dbEx.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();
