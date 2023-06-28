using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Models
{
    public class OrderDetailModel
    {
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        public int OrderId { get; set; }

        public int BookId { get; set; }
        public BookModel? Book { get; set; }

    }

    public class OrderDetailInputModel
    {
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        public int BookId { get; set; }

    }
}
