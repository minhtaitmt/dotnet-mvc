using GenericRepositoryAndUnitofWork.Entities;
using GenericRepositoryAndUnitofWork.Repositories;

namespace GenericRepositoryAndUnitofWork.UnitofWork
{
    public class UnitofWork : IUnitofWork
    {
        private readonly BookStoreContext _context;
        private IBookRepository _bookRepository;
        private ICategoryRepository _categoryRepository;

        public UnitofWork(BookStoreContext context, IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
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
    }
}
