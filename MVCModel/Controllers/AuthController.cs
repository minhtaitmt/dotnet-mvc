using DTO.Models;
using Microsoft.AspNetCore.Mvc;
using MVCModel.Extensions;
using Newtonsoft.Json;
using System.Text;

namespace MVCModel.Controllers
{
    public class AuthController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7160/api");
        private readonly HttpClient _client;

        public AuthController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        #region LOG IN
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(AuthModel model)
        {
            try
            {
                model.Password = EncodePasswordToBase64(model.Password);
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Auth/LogIn", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string serializedData = response.Content.ReadAsStringAsync().Result;
                    var user = JsonConvert.DeserializeObject<UserAuthModel>(serializedData);
                    TempData["successMessage"] = "Log In Successfully!";
                    HttpContext.Session.SetObject("user", user);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();
        }

        [NonAction]
        private static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        #endregion
    }
}
