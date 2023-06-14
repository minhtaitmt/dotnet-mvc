using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericRepositoryAndUnitofWork.Entities
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
