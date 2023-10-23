
using System.ComponentModel.DataAnnotations;

namespace StudentServicesPortal.Models
{
    public class AdmissionModel
    {
        [Required(ErrorMessage = "الحقل مطلوب")]
        public string Name { get; set; }

        public string? Father { get; set; }

        public string? Mother { get; set; }

        public string? NationalNumber { get; set; }

    }

    public class AdmissionViewModel : AdmissionModel
    {
        public int Id { get; set; }

        public bool Accepted { get; set; }

        public DateTime AdmissionDate { get; set; }

        public IEnumerable<AttachmentFileViewModel> AttachmentFiles { get; set; }
    }

    public class AttachmentFileViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
