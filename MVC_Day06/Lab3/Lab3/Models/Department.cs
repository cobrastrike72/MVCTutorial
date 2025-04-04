using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab3.Models
{
    public class Department
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DeptId { get; set; }
        public string DeptName { get; set; }

        public int DeptCapacity { get; set; }

        public ICollection<Student> Students { get; set; } = new HashSet<Student>(); // to ensure that there's no duplication 
    }
}
