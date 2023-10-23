using System.ComponentModel.DataAnnotations;

namespace StudentServicesPortal.Models
{
    public class CourseModel
    {
        [Required(ErrorMessage = "الحقل مطلوب")]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? Teacher { get; set; }
    }

    public class CourseViewModel : CourseModel
    {
        public int Id { get; set; }

        public IEnumerable<RegisterViewModel> Registerations { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "حقل الطالب مطلوب")]
        public int AdmissionId { get; set; }

        [Required(ErrorMessage = "حقل المادة مطلوب")]
        public int CourseId { get; set; }
    }

    public class RegisterViewModel : RegisterModel
    {
        public int Id { get; set; }

        public string CourseName { get; set; }

        public DateTime? RegisterationDate { get; set; }

    }
}
