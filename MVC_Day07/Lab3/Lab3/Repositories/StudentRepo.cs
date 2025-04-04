using Lab3.Data;
using Lab3.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Repositories;

public interface IStudentRepo
{
    void AddStudent(Student student);
    void RemoveStudent(Student student);
    void UpdateStudent(Student student);
    Student GetStudentById(int id);
    Student CheckEmailExistForDifferentUserByEmailAndId(string email, int id);
    Student CheckEmailExistForDifferentUserByEmailOnly(string email);
    List<Student> GetAllStudents();
}
public class StudentRepo : IStudentRepo
{
    private readonly ITIDBContext _context;
    public StudentRepo(ITIDBContext itiDBContext)
    {
        _context = itiDBContext; // will recieve it from the Dependency injection container in the program.cs file
    }
    public void AddStudent(Student std)
    {
        _context.Add(std);
        _context.SaveChanges();
    }

    public List<Student> GetAllStudents()
    {
        return _context.Students.Include(s => s.Department).ToList(); 
    }


    public Student GetStudentById(int id)
    {
        return _context.Students.Include(s => s.Department).FirstOrDefault(s => s.Id == id);
    }

    public void RemoveStudent(Student student)
    {
        _context.Remove(student);
        _context.SaveChanges();
    }

    public void UpdateStudent(Student student)
    {
        _context.Update(student); // will know which dbset based on the type of the object (which is student here)
        _context.SaveChanges();
    }
    public Student CheckEmailExistForDifferentUserByEmailAndId(string email, int id)
    {
        return _context.Students.FirstOrDefault(s => s.Email == email && s.Id != id);
    }

    public Student CheckEmailExistForDifferentUserByEmailOnly(string email)
    {
        return _context.Students.FirstOrDefault(s => s.Email == email);
    }
}
