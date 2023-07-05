using GenericRepositoryAndUnitofWork.Entities;

namespace GenericRepositoryAndUnitofWork.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task UpdateUserAsync(int id, User user);
    }
}
