using PizzaShop.Domain.Interfaces;
using PizzaShop.Services;
using PizzaShop.Extensions;
using Microsoft.EntityFrameworkCore;
using PizzaShop.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPizzaService, PizzaService>();
builder.Services.AddDbContext<PizzaShopDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("PizzaShop");
    options.UseSqlite(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseExceptionHandling();
app.UseHttpsRedirection();
app.UseTiming();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
