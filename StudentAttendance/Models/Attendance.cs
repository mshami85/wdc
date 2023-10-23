using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendance.Models
{
    public class Attendance
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, Column(TypeName = "int")]
        public int AdmissionId { get; set; }

        public string StudentName { get; set; }

        [ForeignKey("Session")]
        public int SessionId { get; set; }
        public Session Session { get; set; }
    }
}
