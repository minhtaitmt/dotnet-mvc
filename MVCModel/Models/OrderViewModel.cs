using System.ComponentModel.DataAnnotations;

namespace MVCModel.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public List<OrderDetailModel>? OrderDetails { get; set; }

        public double Total { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class OrderDetailModel
    {
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        public int OrderId { get; set; }

        public int BookId { get; set; }
        public BookViewModel? Book { get; set; }
    }

    public class BestSellerModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        public string? CategoryName { get; set; }
        public int TotalSells { get; set; }
        public double TotalRevenue { get; set; }

        [Range(1, 12)]
        public int Month { get; set; }
    }

    public class BestCategoryModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? CategoryName { get; set; }
        public int TotalSells { get; set; }
        public double TotalRevenue { get; set; }
        [Range(1, 12)]
        public int Month { get; set; }
    }

    public class TotalBookAndCategorySell
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public int TotalCategorySell { get; set; }
        public int TotalBookSell { get; set; }
        public int Month { get; set; }
    }

    public class UnPopularBooks
    {
        public int Id { get; set; }
        public string? BookName { get; set; }
        public double Price { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
    }

    public class PopularCategory
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int OrderInclude { get; set; }
        public int Month { get; set; }

    }

    public class HighestRevenueBooks
    {
        public int Id { get; set; }
        public string? BookName { get; set; }
        public int CategoryId { get; set; }
        public double Revenue { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int Month { get; set; }
    }

    public class MonthlyRevenue
    {
        public int Month { get; set; }
        public int TotalOrder { get; set; }
        public double OrderRevenue { get; set; }
        public int TotalBook { get; set; }
    }

}
