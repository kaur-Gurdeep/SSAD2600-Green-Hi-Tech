using GreenHiTech.Data;
using GreenHiTech.Models;
using GreenHiTech.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add our app context 
builder.Services.AddDbContext<GreenHiTechContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<CartProductRepo>();
builder.Services.AddScoped<ProductRepo>();
builder.Services.AddScoped<CategoryRepo>();



builder.Services.Configure<IdentityOptions>(options => {
    //// Password settings if you want to ensure password strength.
    //options.Password.RequireDigit           = true;
    //options.Password.RequiredLength         = 8;
    //options.Password.RequireNonAlphanumeric = false;
    //options.Password.RequireUppercase       = true;
    //options.Password.RequireLowercase       = false;
    //options.Password.RequiredUniqueChars    = 6;

    // Lockout settings (Freeze 1 minute only to make testing easier)
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 3; // Lock after 
                                                 // 3 consec failed logins
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddScoped<RoleRepo>();
builder.Services.AddScoped<IdentityUserRepo>();
builder.Services.AddScoped<UserRoleRepo>();
builder.Services.AddScoped<UserRepo>();



var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
