using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAdmission.Models
{
    public class AttachmentFile
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(255), Column(TypeName = "nvarchar(255)")]
        public string? Name { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Path { get; set; }

        [ForeignKey("Admission")]
        public int AdmissionId { get; set; }
        public Admission Admission { get; set; }
    }
}
