using Microsoft.EntityFrameworkCore;
using StudentRegister.Models;

namespace StudentRegister.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Registeration> Registerations { get; set; }
    }
}
