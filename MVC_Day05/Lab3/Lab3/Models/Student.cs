using Lab3.Validators;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab3.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required, StringLength(30, MinimumLength = 3)] // minimum length constraint won't be applied in the database --> not every validation here will be applied in the database
        public string Name { get; set; }
        [MyValidator(18, MinimumAge =18)] // custom validator
        public int Age { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Invalid email format")]
        [Remote("IsEmailExist", "Student", ErrorMessage = "Email already exists", AdditionalFields = "Id, Name, Age")] // to use the id in case of update and check wether if the exists email is already related to the same user 
        public string Email { get; set; } // IsEmailExist is a method in the StudentController and Student is the controller name
        [DataType(DataType.Password)]        
        public string Password { get; set; }
        [NotMapped] // won't be mapped to the database
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public int? DeptId { get; set; } // could be null since it's a foreign key 
        [ForeignKey("DeptId")]
        public Department Department { get; set; }
    }
}
