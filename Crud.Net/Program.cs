using System;
using Crud.Net.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
using AspNetCoreHero.ToastNotification;
using NToastNotify;
using AspNetCoreHero.ToastNotification.Extensions;
using Autofac.Core;
using Microsoft.AspNetCore.Authorization;
using Crud.Net.Filter;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
///using Crud.Net.

var builder = WebApplication.CreateBuilder(args);

//registration policy sirveces
//builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
//builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
// register of NOTIFICATIONS
builder.Services.AddRazorPages().AddNToastNotifyNoty(new NotyOptions
{
    ProgressBar = true,
    Timeout = 3000,

});

// Add ToastNotification
builder.Services.AddRazorPages().AddNToastNotifyToastr(new ToastrOptions()
{
    ProgressBar = true,
    PositionClass = ToastPositions.TopRight,
    PreventDuplicates = true,
    CloseButton = true //to user closs tab if need
});
//builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
//builder.Services.RegisterServices();

builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//since we used IdentityRoles in cod then must define it here : replace .AddDefaultIdentity by .AddIdentity then add IdentityRole
//options can be used to add spicifc options for email , pass
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{ 
    options.SignIn.RequireConfirmedAccount = true;

    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._ ";

}).AddEntityFrameworkStores<ApplicationDbContext>(); 

// make the end user able to allow see  result of any apply action without logout
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero;
});

builder.Services.AddMvc().AddNToastNotifyNoty();
builder.Services.AddMvc().AddNToastNotifyToastr();

var app = builder.Build();  //Building start

using var scope = app.Services.CreateScope(); // creating scope
var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerProvider>();
var logger = loggerFactory.CreateLogger("app"); 

try
{
    // define variables to access roles , users in its classes
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    //seeding data to DB
    await Crud.Net.Seeds.DefaultRoles.SeedAsync(roleManager);
    await Crud.Net.Seeds.DefaultUsers.SeedBasicUserAsync(userManager);
    await Crud.Net.Seeds.DefaultUsers.SeedSuperAdminAsync(userManager, roleManager);

    // to returne notifications
    logger.LogInformation("Data seeded");
    logger.LogInformation("Application Started");
}
catch (System.Exception ex)
{
    logger.LogWarning(ex, "An error occurred while seeding data");
}

if (app.Environment.IsDevelopment()) // Configure the HTTP request pipeline.
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see-https://aka.ms/aspnetcore-hsts.
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

app.UseNToastNotify(); // tu define toast notification

app.UseNotyf();

app.MapRazorPages();

app.Run();
