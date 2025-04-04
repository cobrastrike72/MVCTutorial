using Lab3.Data;
using Lab3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        { // model binder creates a Model State dictionay from the request data 
            // and that ModelState tells wether the data is valid or not
            ModelState.Remove("Id"); // to remove the id from the model state since it's auto generated
            if (ModelState.IsValid )
            {
                _db.Add(std);
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            ViewBag.deptList = _db.Departments.ToList();
            return View(std); // send it again in order to fill the fields with the data and not to overwhelm the user to fill them again and the view already has a model of type student so there's no worries
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
            if(ModelState.IsValid)
            {
                tempStd.Name = std.Name;
                tempStd.Age = std.Age;
                tempStd.Email = std.Email;
                tempStd.DeptId = std.DeptId;
                tempStd.Password = std.Password;
                _db.Update(tempStd);
                _db.SaveChanges();
                return RedirectToAction("index");
                
            }
            return View(std);
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

        public IActionResult IsEmailExist(string Email, int Id, string Name) // Name doesn't have to be used but it's just to show that we can use more than additional field look at the student model
       { // for ajax request   --> id will be used in case of update email --> we will get it from the hidden field
            if(Id != 0) // will recive it from the route
            { // in case of update
                var tempStudent = _db.Students.FirstOrDefault(s => s.Email == Email && s.Id != Id);
                if (tempStudent != null)
                {
                    return Json(false);
                }
                return Json(true);
            }
            var student = _db.Students.FirstOrDefault(s => s.Email == Email);
            if (student != null)
            {
                return Json(false);
            }
            return Json(true);
        }
    }
}
