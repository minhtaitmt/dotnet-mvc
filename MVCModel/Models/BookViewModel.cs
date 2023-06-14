using System.ComponentModel.DataAnnotations;

namespace MVCModel.Models
{
    public class BookViewModel
    {
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

    }

    public class CategoryPriceModel
    {
        public double Price { get; set; }
        public string? CategoryName { get; set; }
    }

    public class CategoryWithBookModel
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public BookViewModel? Book { get; set; }
    }

    public class BookSumaryModel
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int BookCount { get; set; }
    }

    public class LinQBookModel
    {
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

        public string? CategoryName { get; set; }
    }
}
