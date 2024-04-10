using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpManager.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public required string Firstname { get; set; }
        [Required]
        [MaxLength(50)]
        public required string Lastname { get; set; }

        public string Fullname => $"{Firstname} {Lastname}";

        [Required]
        [MaxLength(100)]
        public required string Email { get; set; }
        [Required]
        [MaxLength(20)]
        [Column(TypeName = "varchar(20)")]
        [Display(Name = "Phone Number")]
        public string Phonenumber { get; set; } = string.Empty;

        public ICollection<Leave> LeaveHistory { get; set; } = new List<Leave>();
    }

}
