using Lab3.Repositories;

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

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
