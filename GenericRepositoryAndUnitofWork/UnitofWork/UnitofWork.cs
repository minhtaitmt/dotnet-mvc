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
        private IUserRepository _userRepository;
        private IAuthRepository _authRepository;

        public UnitofWork(BookStoreContext context, IBookRepository bookRepository, ICategoryRepository categoryRepository, IOrderRepository orderRepository, IUserRepository userRepository, IAuthRepository authRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _authRepository = authRepository;
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
        public IAuthRepository AuthRepository
        {
            get
            {
                return _authRepository;
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

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_context);
                return _userRepository;
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
