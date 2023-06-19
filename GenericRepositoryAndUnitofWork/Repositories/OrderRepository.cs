using GenericRepositoryAndUnitofWork.Entities;
using GenericRepositoryAndUnitofWork.Models;

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
                result = _context.Orders.ToList();

                foreach(var order in result)
                {
                    List<OrderDetail> details = _context.OrderDetails.Where(detail => detail.OrderId == order.Id)
                    .Join(_context.Books, detail => detail.BookId, book => book.Id,
                    (detail, book) => new OrderDetail
                    {
                        Id = detail.Id,
                        Quantity = detail.Quantity,
                        Price = detail.Price,
                        OrderId = order.Id,
                        BookId = book.Id,
                        Book = book,
                    })
                    .ToList();
                    order.OrderDetails = details;
                }
                
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
                result = _context.Orders.Find(id)!;
                List<OrderDetail> details = _context.OrderDetails.Where(detail => detail.OrderId == result.Id)
                    .Join(_context.Books, detail => detail.BookId, book => book.Id,
                    (detail, book) => new OrderDetail
                    {
                        Id = detail.Id,
                        Quantity = detail.Quantity,
                        Price = detail.Price,
                        OrderId = result.Id,
                        BookId = book.Id,
                        Book = book,
                    })
                    .ToList();
                result.OrderDetails = details;
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
                result = _context.OrderDetails.Where(detail => detail.OrderId == id).ToList();
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
            if (OrderList!.Count() == 0)
            {
                throw new Exception("Can not find any Order!");
            }
            order.CreatedAt = DateTime.Now;
            foreach (var detail in OrderList)
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
                order.OrderDetails!.Add(orderDetail);
                _context.OrderDetails.Add(orderDetail);
            }
            _context.Orders.Add(order);
            
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
                    TotalRevenue = orderGroup.Sum(order => order.Price),
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
                    TotalRevenue = orderGroup.Sum(order => order.Price),
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

                result.TotalBookSell = 0;
                result.TotalCategorySell = queyResult.Count;
                result.Month = month;
                foreach (var item in queyResult)
                {
                    result.TotalBookSell += item.QuantityBookSell;
                    result.Id = item.Id;
                }
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();

            return result;
        }

        public List<UnPopularBooks> GetUnpopularBooks(int month)
        {
            List<UnPopularBooks> result = null;

            Thread thread = new Thread(() =>
            {

                IEnumerable<Book> query = _context.Orders.Join(_context.OrderDetails, order => order.Id, orderDetail => orderDetail.OrderId,
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
                (order, book) => book);

                result = _context.Books.Except(query)
                .Join(_context.Categories, book => book.CategoryId, category => category.Id,
                (book, category) => new UnPopularBooks
                {
                    Id = book.Id,
                    Price = book.Price,
                    BookName = book.Name,
                    Description = book.Description,
                    CategoryName = category.Name,
                })
                .ToList();
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();
            return result;
        }

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
    }
}
