using GenericRepositoryAndUnitofWork.Entities;
using GenericRepositoryAndUnitofWork.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GenericRepositoryAndUnitofWork.UnitofWork
{
    public class UnitofWork : IUnitofWork
    {
        private readonly BookStoreContext _context;
        private IBookRepository _bookRepository;
        private ICategoryRepository _categoryRepository;
        private IOrderRepository _orderRepository;

        public UnitofWork(BookStoreContext context, IBookRepository bookRepository, ICategoryRepository categoryRepository, IOrderRepository orderRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;
        }

        public IBookRepository BookRepository
        {
            get
            {
                if(_bookRepository == null )
                    _bookRepository = new BookRepository(_context);
                return _bookRepository;
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                if(_categoryRepository == null )
                    _categoryRepository = new CategoryRepository(_context);
                return _categoryRepository;
            }
        }

        public IOrderRepository OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                    _orderRepository = new OrderRepository(_context);
                return _orderRepository;
            }
        }

        public void SaveChanges()
        {
            Thread thread = new Thread(() =>
            {
                _context.SaveChanges();
            });
            thread.IsBackground = false;
            thread.Start();
            thread.Join();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
    }
}
