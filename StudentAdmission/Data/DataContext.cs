using Microsoft.EntityFrameworkCore;
using StudentAdmission.Models;

namespace StudentAdmission.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Admission> Admissions { get; set; }
        public DbSet<AttachmentFile> Attachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admission>()
                        .HasMany(ad => ad.Attachments)
                        .WithOne(at => at.Admission)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Admission>()
                        .HasIndex(ad => ad.Name)
                        .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
