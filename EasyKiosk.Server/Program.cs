using EasyKiosk.Core.Repositories;
using EasyKiosk.Core.Services;
using EasyKiosk.Infrastructure.Context;
using EasyKiosk.Infrastructure.Repositories;
using EasyKiosk.Server.Manager;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// builder.Services.AddDbContext<EasyKioskDbContext>(
//     options => options.UseMySql(
//         connectionString,
//         ServerVersion.AutoDetect(connectionString)
//         ));

builder.Services.AddDbContextFactory<EasyKioskDbContext>(
    options => options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
        ));


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddTransient<IMenuService, MenuService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();