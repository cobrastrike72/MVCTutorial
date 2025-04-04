using System.ComponentModel.DataAnnotations;

namespace Lab3.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
