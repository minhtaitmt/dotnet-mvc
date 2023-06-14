using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryAndUnitofWork.Entities
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext(DbContextOptions <BookStoreContext> opt) : base(opt){ }

        #region DbSet
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        #endregion
    }
}
