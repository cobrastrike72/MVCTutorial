using Lab3.Data;
using Lab3.Models;
using Lab3.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace Lab3.Controllers
{
    public class AccountController : Controller
    {
        private readonly ITIDBContext _dbContext;
        public AccountController(ITIDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel usr)
        {
            if (ModelState.IsValid)
            {
                var userDB = _dbContext.Users.Include(u => u.Roles).FirstOrDefault(s => s.Name == usr.UserName && s.Password == usr.Password);
                if (userDB != null) 
                { 
                    Claim c1 = new Claim(ClaimTypes.Name,userDB.Name);
                    Claim c2 = new Claim(ClaimTypes.Email, userDB.Email);
                    List<Claim> claims = new List<Claim>();
                    foreach (var role in userDB.Roles)
                    {
                        Claim c = new Claim(ClaimTypes.Role, role.Name);
                        claims.Add(c);
                    }

                    ClaimsIdentity ci = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme); // the card --> like user can have many cards in real life national id, driving license, etc.
                    // CookieAuthenticationDefaults.AuthenticationScheme) and this is the type of the card

                    // add the claims to that card
                    ci.AddClaim(c1);
                    ci.AddClaim(c2);
                    foreach (var claim in claims)
                    {
                        ci.AddClaim(claim);
                    }


                    ClaimsPrincipal cp = new ClaimsPrincipal(ci); // the person who has the card and that user can has many cards
                    cp.AddIdentity(ci); // add the card to the user

                    HttpContext.SignInAsync(cp); // sign in the user and create a cookie for him and that cookie will be stored in the client side and will be sent with each upcomming request to the server

                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", "Invalid UserName or Password!");
                }
            }
             return View(usr);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(); // sign out the user and delete the cookie from the client side
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel usr)
        {
            if(ModelState.IsValid)
            {
                var user = new User()
                {
                    Name = usr.Name,
                    Email = usr.Email,
                    Age = usr.Age,
                    Password = usr.Password
                };
                _dbContext.Users.Add(user);
                
                Role r1 = _dbContext.Roles.FirstOrDefault(r => r.Name == "Admin");
                Role r2 = _dbContext.Roles.FirstOrDefault(r => r.Name == "Instructor");

                user.Roles.Add(r1);
                user.Roles.Add(r2);
                _dbContext.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(usr);
        }
    }
}
