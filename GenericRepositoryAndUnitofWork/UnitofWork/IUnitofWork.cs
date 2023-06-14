using GenericRepositoryAndUnitofWork.Repositories;

namespace GenericRepositoryAndUnitofWork.UnitofWork
{
    public interface IUnitofWork
    {
        IBookRepository BookRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        void SaveChanges();
    }
}
