using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Restaurant.DAL;
using Restaurant.DAL.Implimentation;
using Restaurant.DAL.Interfaces;
using RestaurantUI.Profiles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Restaurant.Utilitiy;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//Add service DbContext
string strCon = builder.Configuration.GetConnectionString("RestaurantStrCon");
builder.Services.AddDbContext<RestaurantDBContext>(options => options.UseSqlServer(strCon));
//// Identitiy
//builder.Services.AddDefaultIdentity<IdentityUser>()
//    .AddEntityFrameworkStores<RestaurantDBContext>();


// Identitiy after I Add ApplicationUser => 

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<RestaurantDBContext>()
    .AddDefaultTokenProviders();


// Stripe

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

// Login before using Authorize pages
builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = "/Identity/Account/Login";
    option.LogoutPath = "/Identity/Account/Logout";
    option.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

    



builder.Services.AddSingleton<IEmailSender, EmailSender>();


// Add service NToastNotify
builder.Services.AddRazorPages().AddNToastNotifyToastr(new ToastrOptions()
{
    ProgressBar = true,
    PositionClass = ToastPositions.TopRight,
    PreventDuplicates = true,
    CloseButton = true
});

// (Crud Operation)Add service CategoryRepository if UnitOfWork not exist
//builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();

// Add service UnitOfWork (Crud Operation)
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

builder.Services.AddAutoMapper(typeof(MappingProfile));




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

string key = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
StripeConfiguration.ApiKey = key;

app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.Run();
