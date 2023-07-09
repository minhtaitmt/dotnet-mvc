using DTO.Models;
using GenericRepositoryAndUnitofWork.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryAndUnitofWork.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IdentityResult> AddUserAsync(RegisterModel model)
        {
            var userExist = await _userManager.FindByNameAsync(model.Username);
            if (userExist != null)
            {
                throw new Exception("User da ton tai!");
            }

            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                throw new Exception("Role cua user khong ton tai!");
            }

            var user = new ApplicationUser
            {
                Fullname = model.Fullname,
                Email = model.Email,
                UserName = model.Username,
                Birthday = model.Birthday,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                throw new Exception("Khong the dang ky ngay luc nay. Vui long thu lai!");
            }
            var finalResult = await _userManager.AddToRoleAsync(user, model.Role);

            if (!finalResult.Succeeded)
            {
                throw new Exception("Khong the dang ky ngay luc nay. Vui long thu lai!");
            }

            return finalResult;
        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            var userExist = await _userManager.FindByIdAsync(id);
            if (userExist == null)
            {
                throw new Exception("User khong ton tai!");
            }

            var result = await _userManager.DeleteAsync(userExist);

            if (!result.Succeeded)
            {
                throw new Exception("Khong the xoa user ngay luc nay. Vui long thu lai!");
            }
            return result;
        }

        public async Task<IdentityResult> UpdateUserAsync(string id, UserUpdateInputModel model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User Id not found.");
            }
            user.Email = model.Email;
            user.Address = model.Address;
            user.Gender = model.Gender;
            user.Birthday = model.Birthday;
            user.PhoneNumber = model.PhoneNumber;
            user.Fullname = model.Fullname;

            var result = await _userManager.UpdateAsync(user);
            return result;
        }
    }
}
