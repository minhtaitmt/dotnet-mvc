using DTO.Models;
using Microsoft.AspNetCore.Identity;

namespace GenericRepositoryAndUnitofWork.Repositories
{
    public interface IAuthRepository
    {
        Task<IdentityResult> RegisterAsync(RegisterModel model);
        Task<UserAuthModel> LogInAsync(AuthModel model);
        Task<IdentityResult> DeleteUserAsync(string username);
    }
}
