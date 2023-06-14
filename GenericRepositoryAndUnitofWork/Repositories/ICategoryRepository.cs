using GenericRepositoryAndUnitofWork.Entities;

namespace GenericRepositoryAndUnitofWork.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategoryAsync();
        Task <Category> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);  
        Task UpdateCategoryAsync(int id, Category category);
    }
}
