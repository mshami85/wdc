using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendance.Models
{
    public class Session
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }

        public DateTime? SessionDate { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? CourseName { get; set; }

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
