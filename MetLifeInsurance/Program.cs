using Hangfire;
using MetLifeInsurance;
using MetLifeInsurance.BackgoundService;
using MetLifeInsurance.Controllers;
using MetLifeInsurance.DapperConfigure;
using MetLifeInsurance.Data;
using MetLifeInsurance.Models;
using MetLifeInsurance.Repository;
using MetLifeInsurance.Repository.AccountRepository;
using MetLifeInsurance.Repository.RegisterRepository.AccountRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.UI;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();


builder.Services.AddScoped<IDataAccessLayer, DataAccessLayer>();

builder.Services.AddTransient<IDashboardRepository, DashboardRepository>();
builder.Services.AddTransient<IRegisterRepository, RegisterRepository>();
builder.Services.AddTransient<ITestingRepository, TestingRepository>();
builder.Services.AddScoped<BackgroundServiceJob>();


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("connection")));


builder.Services.AddTransient<IHttpContextAccessor,HttpContextAccessor>();

builder.Services.AddDistributedMemoryCache();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();


builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


builder.Services.AddHangfire(options =>
{
    options.UseSqlServerStorage(builder.Configuration.GetConnectionString("connection"));
});


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(Convert.ToInt32(builder.Configuration["SessionTimeOutMinutes"]));
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddLogging(builder =>
{
    builder.AddConsole(); 
    builder.AddEventLog();        
});



builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();


var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseRouting();


app.UseHangfireDashboard();
app.UseHangfireServer();


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
