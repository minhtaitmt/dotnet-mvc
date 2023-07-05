using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GenericRepositoryAndUnitofWork.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Fullname must be 100 characters or less")]
        public string? Fullname { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required]
        [MaxLength(4), MinLength(2)]
        public string? Gender { get; set; }

        [Required]
        [MaxLength(256, ErrorMessage = "Address must be 256 characters or less")]
        public string? Address { get; set; }
    }
}
