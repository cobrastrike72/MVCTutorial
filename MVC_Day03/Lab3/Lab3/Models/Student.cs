using System.ComponentModel.DataAnnotations.Schema;

namespace Lab3.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public int? DeptId { get; set; } // coould be null since it's a foreign key 
        [ForeignKey("DeptId")]
        public Department Department { get; set; }
    }
}
