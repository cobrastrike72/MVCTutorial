namespace Lab3.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Password { get; set; }
        public virtual List<Role> Roles { get; set; } = new List<Role>();   // virtual to be used with lazy loading
        // new to prevent nulls
    }
}
