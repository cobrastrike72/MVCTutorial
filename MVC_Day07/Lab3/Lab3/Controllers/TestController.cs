using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab3.Controllers
{
    [Authorize(Roles = "Admin, Instructor")] // for any one to access this controller, he should be logged in and must be an admin or instructor
    public class TestController : Controller
    {
        [Authorize(Roles = "Student")] // for any one to access this action, he should be logged in and must be a student and (instructor or admin) 
        public string Display(string name)
        {
            return $"Hello from {name}";
        }

        // for any one to access this action, he should be logged in and must be an admin or instructor
        public string Index()
        {
            return $"Hello form index action";
        }

        [AllowAnonymous] // any one can access this action without being logged in
        public IActionResult Show()
        {
            return Content($"Hello form index action that allows any anonymous user");
        }

    }
}
