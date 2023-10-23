namespace StudentRegister.Dtos
{
    public class CourseModel
    {
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
        public string StudentName { get; set; }

        public int AdmissionId { get; set; }

        public int CourseId { get; set; }
    }

    public class RegisterViewModel : RegisterModel
    {
        public int Id { get; set; }

        public DateTime? RegisterationDate { get; set; }

        public string CourseName { get; set; }
    }
}
