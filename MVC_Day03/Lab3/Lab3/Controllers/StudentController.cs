using Lab3.Data;
using Lab3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Controllers
{
    public class StudentController : Controller
    {
        private ITIDBContext _db;
        public StudentController()
        {
            _db = new ITIDBContext();
        }
        public IActionResult Index()
        {
            var studentList = _db.Students.Include(s => s.Department).ToList();
            return View(studentList);
        }

        public IActionResult Create()
        {
            ViewBag.deptList =  _db.Departments.ToList();
            return View();
        }

        [HttpPost]  
        public IActionResult Create(Student std)
        {
            _db.Add(std);
            _db.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Details(int? id)
        {
            var student = _db.Students.Include(s => s.Department).FirstOrDefault(s => s.Id == id);
            if (student == null) 
            {
                return NotFound();
            }
            return View(student);
        }

        public IActionResult Edit(int? id)
        {
            var student = _db.Students.Include(s => s.Department).FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            ViewBag.deptList = _db.Departments.ToList();
            return View(student);
        }
        [HttpPost]
        public IActionResult Edit(Student std)
        {   //  std id will get it from the routing system --> since it will be go to the same route or url above
            var tempStd = _db.Students.FirstOrDefault(s => s.Id == std.Id);
            tempStd.Name = std.Name;
            tempStd.Age = std.Age;
            tempStd.Email = std.Email;
            tempStd.DeptId = std.DeptId;
            _db.Update(tempStd);
            _db.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Delete(int? id)
        {
            var tempStd = _db.Students.FirstOrDefault(s => s.Id == id);
            if(tempStd == null)
            {
                return NotFound();
            }
            _db.Remove(tempStd);
            _db.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
