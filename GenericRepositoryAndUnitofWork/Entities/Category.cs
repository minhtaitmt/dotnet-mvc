using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericRepositoryAndUnitofWork.Entities
{
    [Table("Category")]
    public class Category
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        public virtual ICollection<Book>? Books { get; set; }
    }
}
