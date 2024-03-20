﻿using Microsoft.AspNetCore.Authentication.Cookies;
using SV20T1020293.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews()
    .AddMvcOptions(option =>
    {
        option.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.Cookie.Name = "AuthenticationCookie";
                    option.LoginPath = "/Account/Login";
                    option.AccessDeniedPath = "/Account/AccessDenined"; //Người sử dụng không thỏa mãn quyền
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(120);  //Thời gian timeout của một phiên đăng nhập
                });

builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(60);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

ApplicationContext.Configure
(
    httpContextAccessor: app.Services.GetRequiredService<IHttpContextAccessor>(),
    hostEnvironment: app.Services.GetService<IWebHostEnvironment>()
);

string connectionString = @"server=LAPCN-DUYAN\SQLEXPRESS;user id=sa;password=123;database=LiteCommerceDB;TrustServerCertificate=true";
SV20T1020293.BusinessLayers.Configuration.Initialize(connectionString);

app.Run();
