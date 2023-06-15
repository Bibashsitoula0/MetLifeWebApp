using MetLifeInsurance.Controllers;
using MetLifeInsurance.DapperConfigure;
using MetLifeInsurance.Data;
using MetLifeInsurance.Models;
using MetLifeInsurance.Repository;
using MetLifeInsurance.Repository.AccountRepository;
using MetLifeInsurance.Repository.RegisterRepository.AccountRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.UI;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();


builder.Services.AddScoped<IDataAccessLayer, DataAccessLayer>();

builder.Services.AddTransient<IDashboardRepository, DashboardRepository>();
builder.Services.AddTransient<IRegisterRepository, RegisterRepository>();
builder.Services.AddTransient<ITestingRepository, TestingRepository>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("connection")));

/*builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddDefaultTokenProviders().AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();*/

builder.Services.AddTransient<IHttpContextAccessor,HttpContextAccessor>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(Convert.ToInt32(builder.Configuration["SessionTimeOutMinutes"]));
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

});
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

/*
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Set the login path
        options.AccessDeniedPath = "/Account/AccessDenied"; // Set the access denied path
    });*/




var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

/*app.UseMiddleware<CustomAuthorization>();*/
app.UseRouting();

app.UseSession();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();


app.MapDefaultControllerRoute();
app.Run();
