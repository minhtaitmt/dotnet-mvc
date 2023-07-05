using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericRepositoryAndUnitofWork.Entities
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }


        public double Total { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual List<OrderDetail>? OrderDetails { get; set; }
    }
}
