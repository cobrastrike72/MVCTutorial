using Lab3.Data;
using Lab3.Models;
using Lab3.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace Lab3.Controllers;
public class StudentController : Controller
{
    private IStudentRepo _studentRepo;
    private IDepartmentRepo _departmentRepo;
    public StudentController(IStudentRepo studentRepo, IDepartmentRepo departmentRepo)
    {
        _studentRepo = studentRepo;
        _departmentRepo = departmentRepo;
    }
    public IActionResult Index()
    {
        var studentList = _studentRepo.GetAllStudents();
        return View(studentList);
    }

    public IActionResult Create()
    {
        ViewBag.deptList =  _departmentRepo.GetAllDepartments();
        return View();
    }

    [HttpPost]  
    public IActionResult Create(Student std)
    { // model binder creates a Model State dictionay from the request data 
        // and that ModelState tells wether the data is valid or not
        ModelState.Remove("Id"); // to remove the id from the model state since it's auto generated
        if (ModelState.IsValid )
        {
            _studentRepo.AddStudent(std);
            return RedirectToAction("index");
        }
        ViewBag.deptList = _departmentRepo.GetAllDepartments();
        return View(std); // send it again in order to fill the fields with the data and not to overwhelm the user to fill them again and the view already has a model of type student so there's no worries
    }

    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        var student = _studentRepo.GetStudentById(id.Value);
        if (student == null)
        {
            return NotFound();
        }
        return View(student);
    }

    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        var student = _studentRepo.GetStudentById(id.Value);
        if (student == null)
        {
            return NotFound();
        }
        ViewBag.deptList = _departmentRepo.GetAllDepartments();
        return View(student);
    }
    [HttpPost]
    public IActionResult Edit(Student std)
    {   //  std id will get it from the routing system --> since it will be go to the same route or url above
        var tempStd = _studentRepo.GetStudentById(std.Id);
        if (ModelState.IsValid)
        {
            tempStd.Name = std.Name;
            tempStd.Age = std.Age;
            tempStd.Email = std.Email;
            tempStd.DeptId = std.DeptId;
            tempStd.Password = std.Password;

            _studentRepo.UpdateStudent(tempStd);
            return RedirectToAction("index");
                
        }
        return View(std);
    }

    public IActionResult Delete(int? id)
    {
        var tempStd = _studentRepo.GetStudentById(id.Value);
        if (tempStd == null)
        {
            return NotFound();
        }
        _studentRepo.RemoveStudent(tempStd);
        return RedirectToAction("index");
    }

    public IActionResult IsEmailExist(string Email, int Id, string Name) // Name doesn't have to be used but it's just to show that we can use more than additional field look at the student model
    { // for ajax request   --> id will be used in case of update email --> we will get it from the hidden field
        if(Id != 0) 
        { // in case of update
            var tempStudent = _studentRepo.CheckEmailExistForDifferentUserByEmailAndId(Email, Id);
            if (tempStudent != null)
            {
                return Json(false); // not valid email
            }
            return Json(true); // incase of null then it's a valid email
        }
        var student = _studentRepo.CheckEmailExistForDifferentUserByEmailOnly(Email); // in case of create student
        if (student != null)
        {
            return Json(false);
        }
        return Json(true);
    }
}
