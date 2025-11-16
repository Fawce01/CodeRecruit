using CodeRecruit.Constants;
using CodeRecruit.Data;
using CodeRecruit.Models;
using CodeRecruit.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
});

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    // Can only confirm account by having confirming logic (send confirmation email) if = true
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddRoles<IdentityRole>() // Adds roles
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IRepository<JobPosting>, JobPostingRepository>();
// Build services
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    // identity manager scope
    var services = scope.ServiceProvider;

    // Seed Roles
    RoleSeeder.SeedRolesAsync(services).Wait();
    // Seed Users
    UserSeeder.SeedUsersAsync(services).Wait();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=JobPostings}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
