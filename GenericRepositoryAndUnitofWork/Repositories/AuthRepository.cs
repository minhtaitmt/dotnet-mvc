using DTO.Models;
using GenericRepositoryAndUnitofWork.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GenericRepositoryAndUnitofWork.Repositories
{
    public class AuthRepository : IAuthRepository
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        // LOG IN
        public async Task<UserAuthModel> LogInAsync(AuthModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if(user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                throw new Exception("Sai username hoac password!");
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                expires: DateTime.Now.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha512Signature)
            );

            var result = new UserAuthModel
            {
                Fullname = user.Fullname,
                Username = user.UserName,
                Email = user.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
            return result;
        }

        // REGISTER
        public async Task<IdentityResult> RegisterAsync(RegisterModel model)
        {
            var userExist = await _userManager.FindByNameAsync(model.Username);
            if (userExist != null)
            {
                throw new Exception("User da ton tai!");
            }

            if(!await _roleManager.RoleExistsAsync(model.Role))
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

        public async Task<IdentityResult> DeleteUserAsync(string username)
        {
            var userExist = await _userManager.FindByNameAsync(username);
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
    }
}
