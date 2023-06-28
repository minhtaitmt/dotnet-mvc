using DTO.Models;
using GenericRepositoryAndUnitofWork.Entities;
//using GenericRepositoryAndUnitofWork.Models;

namespace GenericRepositoryAndUnitofWork.Repositories
{
    public interface IBookRepository
    {
        List<Book> GetAllBooks ();
        //Task<Book> GetBookByIdAsync (int id);
        Book GetBookById(int id);
        //Task<List<LinQBookModel>> GetBooksWithLinqAsync(string? name, double? priceFrom, double? priceTo, string? sortBy, int pageIndex = 1);

        // Get Categories in which Book Price > price
        List<CategoryPriceModel> GetCategoryByPrice(double price);

        // Get Book with pagging
        List<LinQBookModel> GetBooksWithPaging(int pageIndex);

        // Get Book sumary by categoryId 
        List<BookSumaryModel> GetBookSumaryByCategory();

        // Get Books between 2 prices
        List<BookModel> GetBookUnion();

        //Get Books with Category Detail
        List<LinQBookModel> GetBookWithCategoryDetail();

        //
        List<CategoryWithBookModel> GetCategoryWithBook();
        void AddBook (Book book);
        void DeleteBook (int id);

        //Task UpdateBookAsync (int id, Book book);
        void UpdateBook(int id, Book book);
        Task<List<Book>> UpdateBookPricesAsync(List<int> bookIds, double newPrice);
    }
}
