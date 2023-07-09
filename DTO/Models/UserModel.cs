using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Models
{
    public class UserModel
    {
        [Required]
        public string? Fullname { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required]
        public string? Gender { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, Phone]
        public string? PhoneNumber { get; set; }

    }

    public class UserUpdateInputModel
    {
        [Required]
        public string? Fullname { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required]
        public string? Gender { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, Phone]
        public string? PhoneNumber { get; set; }
    }

    public class UserAddInputModel
    {
        public int Id { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        public string? Fullname { get; set; }

        public string? Email { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }
    }
}
