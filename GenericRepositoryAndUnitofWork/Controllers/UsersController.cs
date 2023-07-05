using AutoMapper;
using DTO.Models;
using GenericRepositoryAndUnitofWork.Entities;
using GenericRepositoryAndUnitofWork.UnitofWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenericRepositoryAndUnitofWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersController(IUnitofWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _unitOfWork.UserRepository.GetAllUsersAsync();
            if (users == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<List<UserModel>>(users);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserAddInputModel model)
        {
            var user = _mapper.Map<User>(model);

            try
            {
                await _unitOfWork.UserRepository.AddUserAsync(user);
                _unitOfWork.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }
            return CreatedAtAction("GetUserById", new { id = user.Id }, _mapper.Map<UserModel>(user));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateInputModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            var user = _mapper.Map<User>(model);
            try
            {
                await _unitOfWork.UserRepository.UpdateUserAsync(id, user);
                _unitOfWork.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _unitOfWork.UserRepository.DeleteUserAsync(id);
                _unitOfWork.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
