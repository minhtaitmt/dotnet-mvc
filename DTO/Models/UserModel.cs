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
        public int Id { get; set; }

        public string? Username { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        public string? Fullname { get; set; }

        public string? Email { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }
    }

    public class UserUpdateInputModel
    {
        public int Id { get; set; }

        public string? Fullname { get; set; }

        public string? Gender { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }
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
