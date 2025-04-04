using Lab3.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Data
{
    public class ITIDBContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        public ITIDBContext()
        { // this constructor will be used in the program.cs file in case you didn't pass the callback function to include the connection string
            // and in this case you have to define the connection string here int eh OnConfiguring method
        }

        public ITIDBContext(DbContextOptions<ITIDBContext> options) : base(options)
        { // this constructor will be used in the Program.cs file to pass the connection string to the DbContext
        }
        //override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=MVCDB;Trusted_Connection=True; Trust Server Certificate=True;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData([
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Instructor" },
                new Role { Id = 3, Name = "Student" }

            ]);
            base.OnModelCreating(modelBuilder);
        }
    }
}
