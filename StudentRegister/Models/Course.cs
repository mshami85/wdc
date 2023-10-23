using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentRegister.Models
{
    public class Course
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? Description { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? Teacher { get; set; }

        public ICollection<Registeration> Registerations { get; set; }
    }
}
