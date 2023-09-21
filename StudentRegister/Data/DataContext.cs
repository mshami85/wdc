using Microsoft.EntityFrameworkCore;

namespace StudentAdmission.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

    }
}
