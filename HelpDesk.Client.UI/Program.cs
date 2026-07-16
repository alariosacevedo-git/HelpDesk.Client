using ElmahCore;
using ElmahCore.Mvc;
using HelpDesk.Client.Business.Services;
using HelpDesk.Client.Business.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

#region Global Variables
IConfigurationManager _configurationManager = builder.Configuration;
IWebHostEnvironment _webHostEnvironment = builder.Environment;
var currentDirectory = Directory.GetCurrentDirectory();
#endregion Global Variables

#region AppSettings Files
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
	.AddJsonFile($"appsettings.{_webHostEnvironment.EnvironmentName}.json", optional: true)
	.AddEnvironmentVariables();
#endregion AppSettings Files

#region HttpClient
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<HttpClient>();
#endregion HttpClient

#region Global Settings
builder.Services.Configure<HelpDeskClientSettings>(_configurationManager.GetSection("HelpDeskClientSettings"));
builder.Services.Configure<HelpDeskApiSettings>(_configurationManager.GetSection("HelpDeskApiSettings"));
builder.Services.Configure<EmailSettings>(_configurationManager.GetSection("EmailSettings"));
#endregion Global Settings

#region Custom Interfaces
builder.Services.AddSingleton<IAccountService, AccountService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
#endregion Custom Interfaces

#region AddAuthentication
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();
#endregion AddAuthentication


#region Elmah
builder.Services.AddElmah<XmlFileErrorLog>(options =>
{
	options.LogPath = "~/Elmahlog";
});
#endregion Elmah

builder.Services.AddControllersWithViews();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseElmahExceptionPage();
	app.UseHsts();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseElmahExceptionPage();
	app.UseHsts();
}

var _loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();

_loggerFactory.AddFile($"{currentDirectory}\\wwwroot\\Logs\\HelpDesk.Client.txt");

//app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseElmah();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Account}/{action=Login}/{id?}");
app.Run();

//AspNetCoreHostingModel = inprocess
//Elmah HttpContext.RaiseError(new InvalidOperationException("Test"));
//hostingModel = "OutOfProcess"
//dotnet nuget locals all --clear
//https://positiwise.com/blog/how-to-use-cookies-in-asp-net-core