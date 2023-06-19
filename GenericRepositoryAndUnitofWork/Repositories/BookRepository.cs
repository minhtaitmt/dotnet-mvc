using GenericRepositoryAndUnitofWork.Entities;
using GenericRepositoryAndUnitofWork.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GenericRepositoryAndUnitofWork.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext _context;
        public static int PAGE_SIZE { get; set; } = 4;

        public BookRepository(BookStoreContext context) 
        {
            _context = context;
        }

        public List<Book> GetAllBooks()
        {
            List<Book> books = null;
            Thread thread = new Thread(() =>
            {
                books = _context.Books.ToList();
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();
            return books;
        }

        //public async Task<Book> GetBookByIdAsync(int id)
        //{
        //    var result = await _context.Books.FindAsync(id)!;
        //    return result;
        //}

        public Book GetBookById(int id)
        {
            Book result = null;
            Thread thread = new Thread(() =>
            {
                result = _context.Books.Find(id)!;
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join(); 

            return result;
        }

        public async Task<List<LinQBookModel>> GetBooksWithLinqAsync(string? name, double? priceFrom, double? priceTo, string? sortBy, int pageIndex = 1)
        {
            var allBooks = _context.Books.AsQueryable();

            #region Filtering
            if (!string.IsNullOrEmpty(name))
            {
                allBooks = allBooks.Where(book => book.Name!.Contains(name));
            }
            if (priceFrom.HasValue)
            {
                allBooks = allBooks.Where(book => book.Price >= priceFrom);
            }
            if (priceTo.HasValue)
            {
                allBooks = allBooks.Where(book => book.Price <= priceTo);
            }
            #endregion

            #region Sorting
            allBooks = allBooks.OrderBy(book => book.Name);
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "name_desc": allBooks = allBooks.OrderByDescending(book => book.Name); break;
                    case "price_asc": allBooks = allBooks.OrderBy(book => book.Price); break;
                    case "price_desc": allBooks = allBooks.OrderByDescending(book => book.Price); break;
                }
            }
            #endregion

            #region Paging
            allBooks = allBooks.Skip((pageIndex - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            #endregion

            //Nên map ở đây hay map ở controller?
            var result = allBooks.Select(book => new LinQBookModel
            {
                Id = book.Id,
                Name = book.Name,
                Description = book.Description,
                Price = book.Price,
                CategoryId = book.CategoryId,
                CategoryName = book.Category!.Name
            });

            return result.ToList();

        }

        //public async Task<List<LinQBookModel>> GetBooksWithPagingAsync(int pageIndex)
        //{
        //    var allBooks = _context.Books.AsQueryable();

        //    allBooks = allBooks.Skip((pageIndex - 1) * PAGE_SIZE).Take(PAGE_SIZE);

        //    //Nên map ở đây hay map ở controller?
        //    var result = allBooks.Select(book => new LinQBookModel
        //    {
        //        Id = book.Id,
        //        Name = book.Name,
        //        Description = book.Description,
        //        Price = book.Price,
        //        CategoryId = book.CategoryId,
        //        CategoryName = book.Category!.Name
        //    });

        //    return result.ToList();
        //} 

        public List<LinQBookModel> GetBooksWithPaging(int pageIndex)
        {
            List<LinQBookModel> result = null;
            Thread thread = new Thread(() =>
            {
                var allBooks = _context.Books.AsQueryable();
                allBooks = allBooks.Skip((pageIndex - 1) * PAGE_SIZE).Take(PAGE_SIZE);

                result = allBooks.Select(book => new LinQBookModel
                {
                    Id = book.Id,
                    Name = book.Name,
                    Description = book.Description,
                    Price = book.Price,
                    CategoryId = book.CategoryId,
                    CategoryName = book.Category!.Name
                }).ToList();
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();

            return result;
        }


        // Get Book sumary by categoryId
        public List<BookSumaryModel> GetBookSumaryByCategory()
        {
            List<BookSumaryModel> result = null;
            Thread thread = new Thread(() =>
            {
                result = _context.Books.GroupBy(book => book.CategoryId)
                                    .Select(group => new BookSumaryModel
                                    {
                                        CategoryId = group.Key,
                                        CategoryName = group.FirstOrDefault()!.Category!.Name,
                                        BookCount = group.Count(),
                                    }).ToList();
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();

            return result;
        }

        // Get Book with Category detail
        public List<LinQBookModel> GetBookWithCategoryDetail()
        {
            List<LinQBookModel> result = null;
            Thread thread = new Thread(() =>
            {
                result = _context.Books.Join(_context.Categories, book => book.CategoryId, category => category.Id,
                                    (book, category) => new LinQBookModel
                                    {
                                        Id = book.Id,
                                        Name = book.Name,
                                        Description = book.Description,
                                        Price = book.Price,
                                        CategoryId = category.Id,
                                        CategoryName = category.Name
                                    })
                                .OrderBy(book => book.Name)
                                .ThenBy(book => book.Price).ToList();
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();
            return result;
        }


         //Get Categories With Book Detail | LEFT JOIN
        public List<CategoryWithBookModel> GetCategoryWithBook()
        {
            List<CategoryWithBookModel> result = null;
            Thread thread = new Thread(() =>
            {
                result = _context.Categories.GroupJoin(_context.Books, category => category.Id, book => book.CategoryId,
                                    (category, bookGroup) => new
                                    {
                                        category,
                                        bookGroup
                                    }).SelectMany(b => b.bookGroup.DefaultIfEmpty(),
                                    (category, bookGroup) => new CategoryWithBookModel
                                    {
                                        CategoryId = category.category.Id,
                                        CategoryName = category.category.Name,
                                        Book = bookGroup == null ? null : new BookModel
                                        {
                                            Id = bookGroup.Id,
                                            Name = bookGroup.Name,
                                            Description = bookGroup.Description,
                                            Price = bookGroup.Price,
                                            CategoryId = bookGroup.CategoryId,
                                        }
                                    }).ToList();
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();
            return result;
        }



        // Get Categories which contain Books with Book Price > price | DISTINCT
        public List<CategoryPriceModel> GetCategoryByPrice(double price)
        {
            List<CategoryPriceModel> result = null;
            Thread thread = new Thread(() =>
            {
                result = _context.Books.Where(book => book.Price > price)
                               .OrderBy(book => book.Category!.Name)
                               .GroupBy(book => book.Category.Name)
                               .Select(group => new CategoryPriceModel
                               {
                                   Price = group.FirstOrDefault().Price,
                                   CategoryName = group.FirstOrDefault().Category!.Name,
                               }).ToList();
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();
            return result;
        }

        public List<BookModel> GetBookUnion()
        {
            List<BookModel> result = null;
            Thread thread = new Thread(() =>
            {
                var firstBooks = _context.Books.Where(book => book.Price > 90000)
                                               .OrderBy(book => book.Name)
                                               .Select(book => new BookModel
                                               {
                                                   Id = book.Id,
                                                   Name = book.Name,
                                                   Description = book.Description,
                                                   Price = book.Price,
                                                   CategoryId = book.CategoryId,
                                               });

                var secondBooks = _context.Books.Where(book => book.Price < 70000)
                                                .OrderBy(book => book.Name)
                                                .Select(book => new BookModel
                                                {
                                                    Id = book.Id,
                                                    Name = book.Name,
                                                    Description = book.Description,
                                                    Price = book.Price,
                                                    CategoryId = book.CategoryId,
                                                });

                result = firstBooks.Union(secondBooks).ToList();
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();

            return result;
        }




        public void AddBook(Book book)
        {
            var id = book.CategoryId;
            Category category = null;
            Thread thread = new Thread(() =>
            {
                category = _context.Categories.Find(id);
                if (category == null)
                {
                    throw new Exception("Category Not Found!");
                }
                _context.Books.Add(book);
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();
            
        }

        public void UpdateBook(int id, Book book )
        {
            if(id != book.Id)
            {
                throw new Exception("Book Id not found.");
            }
            Thread thread = new Thread(() =>
            {
                _context.Books.Update(book);
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();
        }

        public void DeleteBook(int id)
        {
            Book book = null;
            Thread thread = new Thread(() =>
            {
                book = _context.Books.Find(id)!;
                if (book == null)
                {
                    throw new Exception("Book Id not found.");
                }
                _context.Books.Remove(book);
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();
            
        }
    }
}
