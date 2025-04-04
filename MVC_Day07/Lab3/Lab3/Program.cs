using Humanizer;
using Lab3.Data;
using Lab3.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Lab3;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Register services in the Dependency Injection container (DIC)
        builder.Services.AddScoped<IStudentRepo, StudentRepo>(); // new object will be created for each request 
        builder.Services.AddScoped<IDepartmentRepo, DepartmentRepo>(); // new object will be created for each request
        builder.Services.AddDbContext<ITIDBContext>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped // will create a new object for each request
        ); // if you didn't pass this callback function it will use the default constructor of the DbContext and will use the connection string from the OnConfiguring method in the ITIDBContext class


        builder.Services.AddSession(options => // session ids are stored in the cookie in the client side but session data are stored in the server side
        {
            options.Cookie.Name = "SessionId"; // the key in the cookie that will carry the value of the session id (if you didn't add it it will be .AspNetCore.Session by default) and you can name it as you want
            options.IdleTimeout = TimeSpan.FromHours(3); // the session will be stored in the SERVER-SIDE for 3 hours --> but if you didn't specify the timeout it will be stored for 20 minutes by default
            options.Cookie.MaxAge = TimeSpan.FromHours(3); // the session id in the cookie will be stored in the CLIENT-SIDE for 3 hours
        }); // note that session will be stored in the server side and will be deleted when the session is expired
        // and also session id will be stored in the cookie as a value and by key called .AspNetCore.Session by default but you can change that name and that cookie will be sent with each upcomming request to the server AS IN line 30
        // note the session id stored in the client side is encrypted and you can't read it and it will be sent with each upcomming request to the server


        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(
                options =>
                {
                    options.LoginPath = "/Account/Login"; // the path to the login page
                    options.LogoutPath = "/Account/Logout"; // the path to the logout page
                    options.AccessDeniedPath = "/Account/AccessDenied"; // the path to the access denied page
                    options.ExpireTimeSpan = TimeSpan.FromHours(3); // the cookie will be stored in the CLIENT-SIDE for 3 hours
                    options.SlidingExpiration = true; // if you set it to true it will reset the expiration time each time the user makes a request
                }
            ); // will use cookie for authentication 
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            // from here are the default or (built-in) middlewares
            app.UseExceptionHandler("/Home/Error"); // it has to be in the first middleware in the pipeline in order to catch all exceptions that may occur in the application 
            // because being the first middle ware means it will be the last middleware in the pipeline too
        }

        app.UseStaticFiles(); // if the request is for a static file like css, js, images, etc. it will be served from the wwwroot folder like the site.css file or site.js file

        app.UseRouting(); // it's job is to inspect the httpContext.Request and decide which controller and action should be called based on the request URL
        app.UseSession(); // Enables session handling (must come before authentication & authorization)
        // Some authorization policies may rely on session values (e.g., user roles, tokens, or permissions stored in the session).
        //If UseSession() is placed after UseAuthorization(), authorization middleware won’t have access to session data.


        app.UseAuthentication(); // middleware for login and logout (its job to get user data from the httpContext.Request which was stored in the cookie)
        app.UseAuthorization(); // after deciding which controller and action should be called it will check if the user is authorized to access this controller and action or not
        // but why Routing middleware comes before Authorization middleware? to know what the user is trying to access and then decide if he is authorized to access that controller and action or not
        // If it's placed too late, other middleware (like authentication/authorization) won’t be able to use session values.

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();


        //My custom middlewares
        //app.Use(async (httpContext, next) => // next is just a delegate that points to the next middleware in the pipeline
        // {   // that httpContext is an object that will be created from the kestrel server when a client make a request from his browser and that httpContext will be passed to the first middleware in the pipeline and that object has two main properties Request and Response you can use them to read the request and write the response
        //     await httpContext.Response.WriteAsync("This is the first middleware in the begining\n");
        //     await next(); // or you can use await next.Invoke(httpContext); since next is a delegate
        //     await httpContext.Response.WriteAsync("This is still the first middleware in the end\n");
        // });
        //app.Use(async (httpContext, next) =>
        //{
        //    await httpContext.Response.WriteAsync("This is the second middleware in the begining\n");
        //    await next();
        //    await httpContext.Response.WriteAsync("This is still the second middleware in the end\n");
        //});
        //app.Run(async httpContext =>
        //{
        //    await httpContext.Response.WriteAsync("This is the third middleware and will make a short circute and anything comes after in the middlewares won't be executed\n");
        //});
        //app.Use(async (httpContext, next) =>
        //{
        //    await httpContext.Response.WriteAsync("This is the forth middleware in the begining\n");
        //    await next();
        //    await httpContext.Response.WriteAsync("This is still the forth middleware in the end\n");
        //});

        //app.Run();
    }
}
