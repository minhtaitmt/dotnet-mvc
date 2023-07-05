using GenericRepositoryAndUnitofWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryAndUnitofWork.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookStoreContext _context;

        public CategoryRepository(BookStoreContext context) 
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategoryAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
        }

        public async Task UpdateCategoryAsync(int id, Category category)
        {
            if(id != category.Id)
            {
                throw new Exception("Category Id not found.");
            }
            _context.Categories.Update(category);
        }

        public List<Book> GetCategoryBooks()
        {
            List<Book> books = new List<Book>();
            Parallel.ForEach(_context.Categories, async category =>
            {
                lock (_context.Categories)
                {
                    var book = _context.Books.Where(book => book.CategoryId == category.Id).ToList();
                    books.Concat(book);
                }
            });

            return books;
        }
    }
}
