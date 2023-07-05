using GenericRepositoryAndUnitofWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryAndUnitofWork.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BookStoreContext _context;

        public UserRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
        }

        public async Task UpdateUserAsync(int id, User user)
        {
            var originUser = await _context.Users.FindAsync(user.Id);
            if (originUser == null)
            {
                throw new Exception("User Id not found.");
            }
            originUser.Email = user.Email;
            originUser.Address = user.Address;
            originUser.Gender = user.Gender;
            originUser.Birthday = user.Birthday;
            originUser.Phone = user.Phone;
            originUser.Fullname = user.Fullname;
        }
    }
}
