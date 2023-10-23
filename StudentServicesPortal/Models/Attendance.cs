using System.ComponentModel.DataAnnotations;

namespace StudentServicesPortal.Models
{
    public class SessionModel
    {
        [Required(ErrorMessage = "عنوان الجلسة مطلوب")]
        public string Title { get; set; }

        public DateTime? SessionDate { get; set; }

        [Required(ErrorMessage = "حقل المادة مطلوب")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "حقل المادة مطلوب")]
        public string? CourseName { get; set; }
    }

    public class SessionViewModel : SessionModel
    {
        public int Id { get; set; }

        public IEnumerable<AttendanceModel> Attendances { get; set; } = Enumerable.Empty<AttendanceModel>();
    }

    public class AttendanceModel
    {
        public int AdmissionId { get; set; }

        public string StudentName { get; set; }
    }

    public class AttendanceViewModel : AttendanceModel
    {
        public string SessionTitle { get; set; }

        public string CourseName { get; set; }

        public DateTime? SessionDate { get; set; }
    }
}
