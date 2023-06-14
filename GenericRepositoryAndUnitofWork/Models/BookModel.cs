using GenericRepositoryAndUnitofWork.Entities;
using System.ComponentModel.DataAnnotations;

namespace GenericRepositoryAndUnitofWork.Models
{
    public class BookModel
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

    public class BookSumaryModel
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int BookCount { get; set; }
    }

    public class CategoryPriceModel
    {
        public double Price { get; set; }
        public string? CategoryName { get; set;}
    }

    public class CategoryWithBookModel
    {
        public int CategoryId { get; set;}
        public string? CategoryName { get; set;}
        public BookModel? Book { get; set;}

    }
}


