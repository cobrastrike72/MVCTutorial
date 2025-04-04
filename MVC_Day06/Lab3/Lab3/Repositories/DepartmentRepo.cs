using Lab3.Data;
using Lab3.Models;

namespace Lab3.Repositories;

public interface IDepartmentRepo
{
    void AddDepartment(Department department);
    void RemoveDepartment(int id);
    void UpdateDepartment(Department department);
    Department GetDepartmentById(int id);
    List<Department> GetAllDepartments();
}
public class DepartmentRepo: IDepartmentRepo
{
    private readonly ITIDBContext _context;
    public DepartmentRepo(ITIDBContext iTIDBContext)
    {
        _context = iTIDBContext;
    }
    public void AddDepartment(Department department)
    {
        _context.Add(department);
        _context.SaveChanges();
    }


    public List<Department> GetAllDepartments()
    {
        return _context.Departments.ToList();
    }

    public Department GetDepartmentById(int id)
    {
        return _context.Departments.FirstOrDefault(d => d.DeptId == id);
    }

    public void RemoveDepartment(int id)
    {
        var tempDept = this.GetDepartmentById(id);
        _context.Remove(tempDept);
        _context.SaveChanges();
    }

    public void UpdateDepartment(Department department)
    {
        throw new NotImplementedException(); // to be implemented later
    }
}
