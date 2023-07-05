using DTO.Models;
using GenericRepositoryAndUnitofWork.UnitofWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenericRepositoryAndUnitofWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitofWork _unitOfWork;

        public AuthController(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var result = await _unitOfWork.AuthRepository.RegisterAsync(model);
            if(result.Succeeded)
            {
                return Ok(result.Succeeded);
            }
            return Unauthorized();
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(AuthModel model)
        {
            model.Password = DecodeFrom64(model.Password);
            var result = await _unitOfWork.AuthRepository.LogInAsync(model);
            if (string.IsNullOrEmpty(result.Token))
            {
                return Unauthorized();
            }
            return Ok(result);
        }

        [NonAction]
        private string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var result = await _unitOfWork.AuthRepository.DeleteUserAsync(username);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }
            return NoContent();

        }
    }
}
