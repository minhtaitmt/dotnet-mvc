using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryAndUnitofWork.Entities
{
    [Table("User")]
    [Index(nameof(User.Username), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50),MinLength(6, ErrorMessage = "Username must be at least 6 characters")]
        public string? Username { get; set; }

        [Required]
        [MaxLength(50), MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string? Password { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required]
        [MaxLength(4), MinLength(2)]
        public string? Gender { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Fullname must be 100 characters or less")]
        public string? Fullname { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Email must be 100 characters or less")]
        public string? Email { get; set; }

        [Required]
        [MaxLength(256, ErrorMessage = "Address must be 256 characters or less")]
        public string? Address { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Phone number must be 10 characters")]
        public string? Phone { get; set; }
        public virtual List<Order>? Orders { get; set; }
    }
}
