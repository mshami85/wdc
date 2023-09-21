using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAdmission.Models
{
    public class Admission
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, Column(TypeName = "nvarchar(100)"), MaxLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(100)"), MaxLength(100)]
        public string? Father { get; set; }

        [Column(TypeName = "nvarchar(100)"), MaxLength(100)]
        public string? Mother { get; set; }

        [Column(TypeName = "nvarchar(15)"), MaxLength(15)]
        public string? NationalNumber { get; set; }

        [Required, Column(TypeName = "datetime")]
        public DateTime AdmissionDate { get; set; }

        [Column(TypeName = "bit")]
        public bool Accepted { get; set; }

        public ICollection<AttachmentFile> Attachments { get; set; }
    }


}
