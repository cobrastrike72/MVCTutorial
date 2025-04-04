using Microsoft.AspNetCore.Mvc;

namespace MVC_Day01.Controllers
{
    public class LabController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Display()
        {
            return View();
        }
        public IActionResult CreateForMult()
        {
            return View();
        }

        public IActionResult CreateForAdd()
        {
            return View();
        }

        public int Add(int x, int y)
        {
            return x + y;
        }

        public int mult(int x, int y)
        {
            return x * y;
        }
    }
}
