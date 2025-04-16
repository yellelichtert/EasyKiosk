using System.Text;
using EasyKiosk.Core.Repositories;
using EasyKiosk.Core.Services;
using EasyKiosk.Infrastructure.Auth;
using EasyKiosk.Infrastructure.Context;
using EasyKiosk.Infrastructure.Repositories;
using EasyKiosk.Server.ClientControllers;
using EasyKiosk.Server.DependencyInjection;
using EasyKiosk.Server.Manager;
using EasyKiosk.Server.Options;
using EasyKiosk.Server.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

//Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();

//Services
builder.Services.AddTransient<IMenuService, MenuService>();
builder.Services.AddTransient<IDeviceService, DeviceService>();


//Ui
builder.Services.AddSingleton<INotificationManager, NotificationManager>();

//DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<EasyKioskDbContext>(
    options => options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
        ));



//Authentication
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccesor>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.ConfigureEasyKioskOptions(builder.Configuration);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new()
        {
            ValidateAudience = false,
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration[$"{nameof(DeviceAuthOptions)}:Issuer"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration[$"{nameof(DeviceAuthOptions)}:SecretKey"])),
            ValidateIssuerSigningKey = true
        };
    })
    .AddIdentityCookies();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
});

builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<EasyKioskDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();


//Other
builder.Services.AddSignalR();


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<DeviceHub>("/Hub");
    

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());


app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();


app.UseAntiforgery();
app.UseEndpoints(endpoints
    => endpoints.MapControllers());









app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();