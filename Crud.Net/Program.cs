using System;
using Crud.Net.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Crud.Net.Seeds;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Crud.Net.Migrations;
using Crud.Net.Constant;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;



var builder = WebApplication.CreateBuilder(args);

// DEFINE LIB OF NOTIFICATIONS
builder.Services.AddRazorPages().AddNToastNotifyNoty(new NotyOptions
{
    ProgressBar = true,
    Timeout = 3000
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//since we used IdentityRoles in cod then must define it here : replace .AddDefaultIdentity by .AddIdentity then add IdentityRole

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// define variables to access roles , users in its classes

//var roleManager = builder.Services.AddIdentityCore<RoleManager<IdentityRole>>();
//var userManager = builder.Services.AddIdentityCore<UserManager<IdentityUser>>();


//builder.Services.AddScoped(DefaultRoles.SeedAsync(roleManager));
//builder.Services.AddScoped(DefaultUsers.SeedBasicUserAsync(userManager));
//builder.Services.AddScoped(DefaultUsers.SeedSuperAdminAsync(userManager, roleManager));

var app = builder.Build();

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerProvider>();
var logger = loggerFactory.CreateLogger("app");

try
{
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await Crud.Net.Seeds.DefaultRoles.SeedAsync(roleManager);
    await Crud.Net.Seeds.DefaultUsers.SeedBasicUserAsync(userManager);
    await Crud.Net.Seeds.DefaultUsers.SeedSuperAdminAsync(userManager, roleManager);

    logger.LogInformation("Data seeded");
    logger.LogInformation("Application Started");
}
catch (System.Exception ex)
{
    logger.LogWarning(ex, "An error occurred while seeding data");
}


app.MapRazorPages();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

app.Run();
