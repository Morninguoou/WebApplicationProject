using marian_onsite.Models;
using marian_onsite.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null).AddNewtonsoftJson();

builder.Services
   .AddAuthentication()
   .AddBearerToken();

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<ClaimsPrincipal>(s =>
    s.GetService<IHttpContextAccessor>().HttpContext.User);

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddSingleton<EmailService>();

builder.Services.Configure<UserDatabaseSettings>(
    builder.Configuration.GetSection("MarianOnsiteDatabase"));
builder.Services.AddSingleton<UserService>();

builder.Services.Configure<StudyGroupDatabaseSettings>(
    builder.Configuration.GetSection("MarianOnsiteDatabase"));
builder.Services.AddSingleton<StudyGroupService>();

builder.Services.Configure<RequestJoinStudyGroupDatabaseSettings>(
    builder.Configuration.GetSection("MarianOnsiteDatabase"));
builder.Services.AddSingleton<RequestJoinStudyGroupService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{action=Index}/{id?}",
    defaults: new { controller = "Auth" }
);

app.MapControllerRoute(
    name: "Auth",
    pattern: "Auth/{action=Index}",
    defaults: new { controller = "Auth" });

app.MapControllerRoute(
    name: "StudyGroups",
    pattern: "StudyGroups/{action=Index}/{id?}",
    defaults: new { controller = "StudyGroups" });

app.MapControllerRoute(
    name: "Profile",
    pattern: "Profile/{action=Index}/{id?}",
    defaults: new { controller = "Profile" });

app.Run();
