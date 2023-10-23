
namespace StudentAttendance.Dtos
{
    public class SessionModel
    {
        public string Title { get; set; }

        public DateTime? SessionDate { get; set; }

        public int CourseId { get; set; }

        public string? CourseName { get; set; }
    }

    public class SessionViewModel : SessionModel
    {
        public int Id { get; set; }

        public IEnumerable<AttendanceViewModel> Attendances { get; set; } = Enumerable.Empty<AttendanceViewModel>();
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
