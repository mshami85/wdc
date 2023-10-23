using Microsoft.EntityFrameworkCore;
using StudentAttendance.Models;

namespace StudentAttendance.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
    }
}
