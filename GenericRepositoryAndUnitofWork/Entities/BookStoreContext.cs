
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryAndUnitofWork.Entities
{
    public class BookStoreContext : IdentityDbContext<ApplicationUser>
    {
        public BookStoreContext(DbContextOptions <BookStoreContext> opt) : base(opt){ }

        #region DbSet
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders{ get; set; }
        public DbSet<OrderDetail> OrderDetails{ get; set; }
        public DbSet<User> Users{ get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            AddRoles(builder);
        }

        private void AddRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
                (
                    new IdentityRole() { Name = "admin", ConcurrencyStamp = "1", NormalizedName = "admin" },
                    new IdentityRole() { Name = "user", ConcurrencyStamp = "2", NormalizedName = "user" },
                    new IdentityRole() { Name = "hr", ConcurrencyStamp = "3", NormalizedName = "hr" }
                );
        }
    }


}
