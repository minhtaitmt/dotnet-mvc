using DTO.Models;
using GenericRepositoryAndUnitofWork.Entities;
//using GenericRepositoryAndUnitofWork.Models;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace GenericRepositoryAndUnitofWork.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BookStoreContext _context;

        public OrderRepository(BookStoreContext context)
        {
            _context = context;
        }
        public List<Order> GetAllOrders()
        {
            List<Order> result = null;
            Thread thread = new Thread(() =>
            {
                result = _context.Orders.Include("OrderDetails").Include("OrderDetails.Book").ToList(); // Eager Loading
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();

            return result;
        }

        public Order GetOrderById(int id)
        {
            Order result = null;
            Thread thread = new Thread(() =>
            {
                result = _context.Orders.Include("OrderDetails").Include("OrderDetails.Book").SingleOrDefault(order => order.Id == id)!;
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();

            return result;
        }

        public List<OrderDetail> GetOrderDetailsById(int id)
        {
            List<OrderDetail> result = null;
            Thread thread = new Thread(() =>
            {
                result = _context.OrderDetails.Include("Book").Where(detail => detail.OrderId == id).ToList();
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();

            return result;
        }

        public async Task AddOrder(Order order)
        {
            var OrderList = order.OrderDetails;
            order.OrderDetails = new List<OrderDetail>();
            if (OrderList!.Count == 0)
            {
                throw new Exception("Can not find any Order!");
            }
            order.CreatedAt = DateTime.Now;//.AddMonths(-2);
            foreach(var detail in OrderList)
            {
                var book = await _context.Books.FindAsync(detail.BookId);
                if (book == null)
                {
                    throw new Exception("Book id does not match!");
                }
                var orderDetail = new OrderDetail
                {
                    Quantity = detail.Quantity,
                    Price = book!.Price,
                    OrderId = order.Id,
                    BookId = detail.BookId,
                    Book = book
                };
                order.Total += (book.Price * detail.Quantity);
                
                try
                {
                    order.OrderDetails!.Add(orderDetail);
                    _context.OrderDetails.Add(orderDetail);
                }
                catch (Exception err)
                {
                    Console.WriteLine(err);
                }
            };
            await _context.Orders.AddAsync(order);
        }



        public void DeleteOrder(int id)
        {
            Order order = null;
            Thread thread = new Thread(() =>
            {
                order = _context.Orders.Find(id);
                _context.Orders.Remove(order);
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();
        }

        public void UpdateOrder(int id, Order order)
        {
            throw new NotImplementedException();
        }


        // GET 10 BEST SELLER BOOKS
        public List<BestSellerModel> GetTenBestSellerBooks(int month)
        {
            List<BestSellerModel> result = null;
            Thread thread = new Thread(() =>
            {
                var allOrders = _context.Orders.AsQueryable();

                result = allOrders.Join(_context.OrderDetails, order => order.Id, orderDetail => orderDetail.OrderId,
                    (order, orderDetail) => new
                    {
                        Id = order.Id,
                        Price = orderDetail.Price,
                        BookId = orderDetail.BookId,
                        Quantity = orderDetail.Quantity,
                        Month = order.CreatedAt.Month
                    })
                .Where(order => order.Month == month)
                .Join(_context.Books, order => order.BookId, book => book.Id,
                (order, book) => new
                {
                    Id = order.Id,
                    Price = order.Price,
                    BookId = order.BookId,
                    Quantity = order.Quantity,
                    Month = order.Month,
                    BookName = book.Name,
                    CategoryId = book.CategoryId,
                })
                .Join(_context.Categories, order => order.CategoryId, category => category.Id,
                (order, category) => new
                {
                    Id = order.Id,
                    Price = order.Price,
                    BookId = order.BookId,
                    Quantity = order.Quantity,
                    Month = order.Month,
                    BookName = order.BookName,
                    CategoryName = category.Name,
                })
                .GroupBy(order => order.BookId).Select(orderGroup => new BestSellerModel
                {
                    Id = (int)orderGroup.Key,
                    Name = orderGroup.FirstOrDefault()!.BookName,
                    Price = orderGroup.FirstOrDefault()!.Price,
                    CategoryName = orderGroup.FirstOrDefault()!.CategoryName,
                    TotalSells = orderGroup.Sum(order => order.Quantity),
                    TotalRevenue = orderGroup.Sum(order => (order.Price * order.Quantity)),
                    Month = orderGroup.FirstOrDefault()!.Month
                }).OrderByDescending(order => order.TotalSells).Take(10).ToList();
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();

            return result;
        }

        public List<BestCategoryModel> GetBestSellerCategory(int month)
        {
            List<BestCategoryModel> result = null;
            Thread thread = new Thread(() =>
            {
                var allOrders = _context.Orders.AsQueryable();

                result = allOrders.Join(_context.OrderDetails, order => order.Id, orderDetail => orderDetail.OrderId,
                    (order, orderDetail) => new
                    {
                        Id = order.Id,
                        Price = orderDetail.Price,
                        BookId = orderDetail.BookId,
                        Quantity = orderDetail.Quantity,
                        Month = order.CreatedAt.Month
                    })
                .Where(order => order.Month == month)
                .Join(_context.Books, order => order.BookId, book => book.Id,
                (order, book) => new
                {
                    Id = order.Id,
                    Price = order.Price,
                    BookId = order.BookId,
                    Quantity = order.Quantity,
                    Month = order.Month,
                    CategoryId = book.CategoryId,
                })
                .Join(_context.Categories, order => order.CategoryId, category => category.Id,
                (order, category) => new
                {
                    Id = order.Id,
                    Price = order.Price,
                    Quantity = order.Quantity,
                    Month = order.Month,
                    CategoryName = category.Name,
                    CategoryId = order.CategoryId,
                })
                .GroupBy(order => order.CategoryId).Select(orderGroup => new BestCategoryModel
                {
                    Id = (int)orderGroup.Key,
                    CategoryName = orderGroup.FirstOrDefault()!.CategoryName,
                    TotalSells = orderGroup.Sum(order => order.Quantity),
                    TotalRevenue = orderGroup.Sum(order => (order.Price * order.Quantity)),
                    Month = orderGroup.FirstOrDefault()!.Month
                }).OrderByDescending(order => order.TotalSells).Take(5).ToList();
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();

            return result;
        }

        public TotalBookAndCategorySell GetTotalBookAndCategorySell(int month) 
        {
            List<BookSellModel> queyResult = null;
            TotalBookAndCategorySell result = new TotalBookAndCategorySell();

            Thread thread = new Thread(() =>
            {
                var query = _context.Orders.AsQueryable();
                queyResult = query.Join(_context.OrderDetails, order => order.Id, orderDetail => orderDetail.OrderId,
                    (order, orderDetail) => new
                    {
                        Id = order.Id,
                        Price = orderDetail.Price,
                        BookId = orderDetail.BookId,
                        Quantity = orderDetail.Quantity,
                        Month = order.CreatedAt.Month
                    })
                .Where(order => order.Month == month)
                .Join(_context.Books, order => order.BookId, book => book.Id,
                (order, book) => new
                {
                    Id = order.Id,
                    Quantity = order.Quantity,
                    CategoryId = book.CategoryId,
                })
                .GroupBy(order => order.CategoryId)
                .Select(orderGroup => new BookSellModel
                {
                    Id = orderGroup.Key,
                    QuantityBookSell = orderGroup.Sum(order => order.Quantity),
                }).ToList();

                result.TotalBookSell = queyResult.Sum(order => order.QuantityBookSell);
                result.TotalCategorySell = queyResult.Count;
                result.Month = month;
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();

            return result;
        }


        //public List<UnPopularBooks> GetUnpopularBooks(int month)
        //{
        //    List<UnPopularBooks> result = null;

        //    Thread thread = new Thread(() =>
        //    {

        //        IEnumerable<Book> query = _context.Orders.Join(_context.OrderDetails, order => order.Id, orderDetail => orderDetail.OrderId,
        //            (order, orderDetail) => new
        //            {
        //                Id = order.Id,
        //                Price = orderDetail.Price,
        //                BookId = orderDetail.BookId,
        //                Quantity = orderDetail.Quantity,
        //                Month = order.CreatedAt.Month
        //            })
        //        .Where(order => order.Month == month)
        //        .Join(_context.Books, order => order.BookId, book => book.Id,
        //        (order, book) => book);

        //        result = _context.Books.Except(query)
        //        .Join(_context.Categories, book => book.CategoryId, category => category.Id,
        //        (book, category) => new UnPopularBooks
        //        {
        //            Id = book.Id,
        //            Price = book.Price,
        //            BookName = book.Name,
        //            Description = book.Description,
        //            CategoryName = category.Name,
        //        })
        //        .ToList();
        //    });
        //    thread.IsBackground = false;
        //    thread.Start();
        //    thread.Join();
        //    return result;
        //}



        public Book GetBestSellerBook(int month)
        {
            var result = new Book();
            Thread thread = new Thread(() =>
            {
                result = _context.Orders.Join(_context.OrderDetails, order => order.Id, detail => detail.OrderId,
                (order, orderDetail) => new
                {
                    Id = order.Id,
                    Price = orderDetail.Price,
                    BookId = orderDetail.BookId,
                    Quantity = orderDetail.Quantity,
                    Month = order.CreatedAt.Month
                })
                .Where(order => order.Month == month)
                .GroupBy(order => order.BookId).Select(group => new
                {
                    BookId = group.Key,
                    Quantity = group.Sum(order => order.Quantity),
                }).OrderByDescending(order => order.Quantity)
                .Join(_context.Books, order => order.BookId, book => book.Id,
                (order, book) => book).FirstOrDefault();
                if(result == null)
                {
                    result = new Book();
                }
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();
            return result;
        }

        ////// LINQ QUERY SYNTAX AND ASYNC FUNCTION
        public async Task<List<BestSellerModel>> GetTenBestSellerBooksAsync(int month)
        {
            var query = from order in _context.Orders
                        join detail in _context.OrderDetails on order.Id equals detail.OrderId
                        where order.CreatedAt.Month == month
                        join book in _context.Books on detail.BookId equals book.Id
                        join category in _context.Categories on book.CategoryId equals category.Id
                        group new { order, detail, book, category } by new { book.Id, book.Name, CategoryName = category.Name, Month = order.CreatedAt.Month } into bookGroup
                        orderby bookGroup.Sum(x => x.detail.Quantity) descending
                        select new BestSellerModel
                        {
                            Id = bookGroup.Key.Id,
                            Name = bookGroup.Key.Name,
                            Price = bookGroup.FirstOrDefault()!.detail.Price,
                            CategoryName = bookGroup.Key.CategoryName,
                            TotalSells = bookGroup.Sum(x => x.detail.Quantity),
                            TotalRevenue = bookGroup.Sum(x => x.detail.Price * x.detail.Quantity),
                            Month = bookGroup.Key.Month
                        };

            return await query.Take(10).ToListAsync();
        }

        public async Task<List<UnPopularBooks>> GetUnpopularBooks(int month)
        {
            var query = from category in _context.Categories
                        join book in (from book in _context.Books
                                      where !(from detail in _context.OrderDetails select detail.BookId).Contains(book.Id)
                                      select book)
                        on category.Id equals book.CategoryId
                        select new UnPopularBooks
                        {
                            Id = book.Id,
                            Price = book.Price,
                            BookName = book.Name,
                            Description = book.Description,
                            CategoryName = category.Name,
                        };
            return await query.ToListAsync();
        }

        // Top 5 cuốn sách có doanh thu cao nhất trong tháng
        public async Task<List<HighestRevenueBooks>> GetHighestRevenueBooksAsync(int month)
        {
            var query = from order in _context.Orders
                        join detail in _context.OrderDetails on order.Id equals detail.OrderId
                        where order.CreatedAt.Month == month
                        join book in _context.Books on detail.BookId equals book.Id
                        select new
                        {
                            Id = detail.Id,
                            Price = detail.Price,
                            BookId = detail.BookId,
                            BookName = book.Name,
                            CategoryId = book.CategoryId,
                            Quantity = detail.Quantity,
                            Month = order.CreatedAt.Month
                        } into orderBook
                        group orderBook by orderBook.BookId into bookGroup
                        orderby bookGroup.Sum(book => (book.Price * book.Quantity)) descending
                        select new HighestRevenueBooks
                        {
                            Id = bookGroup.Key,
                            BookName = bookGroup.FirstOrDefault()!.BookName,
                            CategoryId = bookGroup.FirstOrDefault()!.CategoryId,
                            Revenue = bookGroup.Sum(book => (book.Price * book.Quantity)),
                            Quantity = bookGroup.Sum(book => book.Quantity),
                            Price = bookGroup.FirstOrDefault()!.Price,
                            Month = bookGroup.FirstOrDefault()!.Month
                        };

            return await query.Take(5).ToListAsync();
        }

        // Doanh thu của từng tháng trong năm
        public async Task<List<MonthlyRevenue>> GetMonthlyRevenueAsync(int year)
        {
            var query = from order in _context.Orders
                        join detail in _context.OrderDetails on order.Id equals detail.OrderId
                        where order.CreatedAt.Year == year
                        select new
                        {
                            Id = order.Id,
                            Quantity = detail.Quantity,
                            Total = order.Total,
                            Month = order.CreatedAt.Month,
                        } into orderDetail
                        group orderDetail by orderDetail.Id into orderGroup
                        select new
                        {
                            Id = orderGroup.Key,
                            Month = orderGroup.FirstOrDefault()!.Month,
                            TotalOrder = orderGroup.Count(),
                            OrderRevenue = orderGroup.FirstOrDefault()!.Total,
                            TotalBook = orderGroup.Sum(order => order.Quantity),
                        } into orderTotal
                        group orderTotal by orderTotal.Month into result
                        select new MonthlyRevenue
                        {
                            Month = result.Key,
                            TotalOrder = result.Count(),
                            OrderRevenue = result.Sum(order => order.OrderRevenue),
                            TotalBook = result.Sum(order => order.TotalBook),
                        };

            return await query.ToListAsync();
        }


        // Top 3 categories mà sách của nó có mặt trong nhiều order nhất trong tháng
        public async Task<List<PopularCategory>> GetPopularCategoriesAsync(int month)
        {
            var query = from order in _context.Orders
                        join detail in _context.OrderDetails on order.Id equals detail.OrderId
                        where order.CreatedAt.Month == month && order.CreatedAt.Year == DateTime.Now.Year
                        join book in _context.Books on detail.BookId equals book.Id
                        select new
                        {
                            DetailId = detail.Id,
                            OrderId = order.Id,
                            BookId = detail.BookId,
                            CategoryId = book.CategoryId,
                            Month = order.CreatedAt.Month,
                        } into orderBook
                        group orderBook by new { orderBook.CategoryId } into orderGroup
                        orderby orderGroup.Count() descending
                        select new
                        {
                            CategoryId = orderGroup.FirstOrDefault()!.CategoryId,
                            Month = orderGroup.FirstOrDefault()!.Month,
                            OrderInclude = orderGroup.Count()
                        } into bookOrder
                        join category in _context.Categories on bookOrder.CategoryId equals category.Id
                        select new PopularCategory
                        {
                            CategoryId = bookOrder.CategoryId,
                            CategoryName = category.Name,
                            Month = bookOrder.Month,
                            OrderInclude = bookOrder.OrderInclude
                        };
            return await query.Take(5).ToListAsync();
        }
    }
}
