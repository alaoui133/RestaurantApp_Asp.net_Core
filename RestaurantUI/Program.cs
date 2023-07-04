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

//Add Connection String

string strCon = builder.Configuration.GetConnectionString("RestaurantStrCon");

//Add service DbContext

builder.Services.AddDbContext<RestaurantDBContext>(options => options.UseSqlServer(strCon));
//// Identitiy
//builder.Services.AddDefaultIdentity<IdentityUser>()
//    .AddEntityFrameworkStores<RestaurantDBContext>();


// Identitiy after I Add ApplicationUser Class  => 

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
// Mapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add Sessions Services

builder.Services.AddDistributedMemoryCache(); // Store Items to memory
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromSeconds(100);
    option.Cookie.HttpOnly = true; //Cookie is accessible by client Side script
    option.Cookie.IsEssential = true;// if is essential for the app to function correctly
});






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
app.UseSession();   

app.MapRazorPages();

app.Run();
