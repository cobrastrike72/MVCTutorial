using Microsoft.AspNetCore.Mvc;

namespace Lab3.Controllers;

public class StateManagment : Controller
{
    public IActionResult SetStateByCookies()
    {
        // httpContext is the object created from the kestrel web server when client sends a request from thier browser
        // and that object has two main properties Request and Response you can use them to read the request and write the response to be sent to the client
        int id = 20;
        string name = "DOD";
        ViewBag.id = id;
        ViewBag.name = name;
        HttpContext.Response.Cookies.Append("id", id.ToString(), new CookieOptions() { Expires = DateTime.Now.AddHours(3)}); // will save the cookie for 3 hours but if you didn't pass the cookie options it will be saved for the current session only and when you cloce the browser it will be deleted
        HttpContext.Response.Cookies.Append("name", name, new CookieOptions() { Expires = DateTime.Now.AddHours(3) });
        // or for simplicity you can write the below but internally the Response and Request are properties in the HttpContext object you can use them directly
        // Response.Cookies.Append("id", id.ToString());
        // Response.Cookies.Append("name", name);
        return View();
        // note that cookies are stored in the client side and will be sent with each upcomming request to the server
    }

    public IActionResult GetStateFromCookies()
    {
        // to read the cookies from the httpContext.Request object
        ViewBag.id = HttpContext.Request.Cookies["id"];
        ViewBag.name = HttpContext.Request.Cookies["name"];
        // or for simpicity you can use the below directly
        //ViewBag.id = Request.Cookies["id"];
        //ViewBag.name = Request.Cookies["name"];
        return View();
    }


    public IActionResult SetStateBySession()
    {
        int id = 20;
        string name = "DOD";
        ViewBag.id = id;
        ViewBag.name = name;
        HttpContext.Session.SetInt32("id", id); // those data will be stored in the server side and will be deleted when the session is expired
        HttpContext.Session.SetString("name", name);
        // and in the client-side the session id will be stored in a cookie called .AspNetCore.Session and that cookie will be sent with each upcomming request to the server
        // and note to use Session you have to add a middleware called UseSession() in the pipeline in the program.cs file and it should be after the authorizaton middleware in order to work
        // and you have to add also service in the Dependency Injection container in the program.cs file related to the session called builder.Services.AddSession() --> and you can add options to the session like the timeout for example this means how long the session will be stored in the server side
        // and options.Cookie.MaxAge means how long the session id will be stored in the client side
        // and options.Cookie.Name means the name of the key in the cookie that will carry the value of the session id
        return View();
    }

    public IActionResult GetStateFromSession()
    {
        ViewBag.id = HttpContext.Session.GetInt32("id");
        ViewBag.name = HttpContext.Session.GetString("name");
        return View();
    }



}
