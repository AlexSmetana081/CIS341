using Lab05.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Lab05.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Add common Identity services (login, registration, etc.)
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Add role-related services
    .AddEntityFrameworkStores<WorkoutAuthenticationContext>();
//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    // Define a policy that requires user to be in a specific role 
    // -- used for View-based authorization
    options.AddPolicy("RequireAdministratorRole", policy =>
        policy.RequireRole("Admin"));
});

// Add DbContext to the service container.
builder.Services.AddDbContext<WorkoutContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<WorkoutAuthenticationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WorkoutAuthenticationContextConnection")));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<WorkoutAuthenticationContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});


builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.Cookie.Name = "YourAppCookieName";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Identity/Account/Login";
    // ReturnUrlParameter requires 
    //using Microsoft.AspNetCore.Authentication.Cookies;
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});

// using Microsoft.AspNetCore.Identity;

builder.Services.Configure<PasswordHasherOptions>(option =>
{
    option.IterationCount = 12000;
});

// Force Identity's security stamp to be validated every minute.
builder.Services.Configure<SecurityStampValidatorOptions>(o =>
                   o.ValidationInterval = TimeSpan.FromMinutes(1));

var app = builder.Build();

// Since the DbContext is a scoped service, we need to create a scope to retrieve the service.
using (var scope = app.Services.CreateScope())
{
    // Service provider for the scope
    var services = scope.ServiceProvider;
    try
    {
        // Get the DbContext from the service provider
        var context = services.GetRequiredService<WorkoutContext>();
        var authenticationContext = services.GetRequiredService<WorkoutAuthenticationContext>();
        DbInitializer.Initialize(context, authenticationContext);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    // Access the DbContext from service provider container
    var context = services.GetRequiredService<WorkoutAuthenticationContext>();

    // Apply any pending migrations to database. 
    // Creates the database if it doesn't exist.
    context.Database.Migrate();

    try
    {
        // Initialize the users and roles
        InitializeUsersRoles.Initialize(services).Wait();
    }
    catch (Exception ex)
    {
        // Something went wrong
        var logger = services.GetRequiredService<ILogger<Program>>();
        string errorMessage = $"An error occurred seeding the users and roles: {ex.Message}";
        logger.LogError(errorMessage);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
