using Microsoft.AspNetCore.Mvc;
using MVCModel.Models;
using Newtonsoft.Json;
using System.Configuration;

namespace MVCModel.Controllers
{
    public class OrderController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7160/api");
        private readonly HttpClient _client;

        public OrderController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<OrderViewModel> orders = new List<OrderViewModel>();
            HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Orders").Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                orders = JsonConvert.DeserializeObject<List<OrderViewModel>>(data);
            }
            return View(orders);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            OrderViewModel order = new OrderViewModel();
            HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Orders/" + id).Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                order = JsonConvert.DeserializeObject<OrderViewModel>(data);
            }
            return View(order);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpGet]
        public IActionResult GetTenBestSellerBook(int month)
        {
            if (month < 1 || month > 12)
            {
                month = DateTime.Now.Month;
            }
            List<BestSellerModel> books = new List<BestSellerModel>();
            HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Orders/bestSeller/month/" + month).Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                books = JsonConvert.DeserializeObject<List<BestSellerModel>>(data);
            }
            return View(books);
        }

        [HttpGet]
        public IActionResult GetFiveBestSellerCategories(int month)
        {
            if (month < 1 || month > 12)
            {
                month = DateTime.Now.Month;
            }
            List<BestCategoryModel> categories = new List<BestCategoryModel>();
            HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Orders/bestSellerCate/month/" + month).Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                categories = JsonConvert.DeserializeObject<List<BestCategoryModel>>(data);
            }
            return View(categories);
        }

        [HttpGet]
        public IActionResult GetTotalBookAndCategorySell(int month)
        {
            if (month < 1 || month > 12)
            {
                month = DateTime.Now.Month;
            }
            TotalBookAndCategorySell total = new TotalBookAndCategorySell();
            HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Orders/TotalBookAndCategory/month/" + month).Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                total = JsonConvert.DeserializeObject<TotalBookAndCategorySell>(data);
            }
            return View(total);
        }

        [HttpGet]
        public IActionResult GetUnpopularBooks(int month)
        {
            if (month < 1 || month > 12)
            {
                month = DateTime.Now.Month;
            }
            List<UnPopularBooks> books = new List<UnPopularBooks>();
            HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Orders/UnPopularBook/month/" + month).Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                books = JsonConvert.DeserializeObject<List<UnPopularBooks>>(data);
            }
            return View(books);
        }

        [HttpGet]
        public IActionResult GetBestSellerBook(int month)
        {
            if (month < 1 || month > 12)
            {
                month = DateTime.Now.Month;
            }
            BookViewModel book = new BookViewModel();
            HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Orders/BestSellerBook/month/" + month).Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                book = JsonConvert.DeserializeObject<BookViewModel>(data);
            }
            return View(book);
        }

        [HttpGet]
        public IActionResult GetHighestRevenueBooks(int month)
        {
            if (month < 1 || month > 12)
            {
                month = DateTime.Now.Month;
            }
            List<HighestRevenueBooks> book = new List<HighestRevenueBooks>();
            HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Orders/highestRevenueBooks/month/" + month).Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                book = JsonConvert.DeserializeObject<List<HighestRevenueBooks>>(data);
            }
            return View(book);
        }

        [HttpGet]
        public IActionResult GetMonthlyRevenue(int year)
        {
            if (year < 2000 || year > DateTime.Now.Year)
            {
                year = DateTime.Now.Year;
            }
            List<MonthlyRevenue> revenue = new List<MonthlyRevenue>();
            HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Orders/monthlyRevenue/year/" + year).Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                revenue = JsonConvert.DeserializeObject<List<MonthlyRevenue>>(data);
            }
            return View(revenue);
        }

        [HttpGet]
        public IActionResult GetPopularCategories(int month)
        {
            if (month < 1 || month > 12)
            {
                month = DateTime.Now.Month;
            }
            List<PopularCategory> categories = new List<PopularCategory>();
            HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Orders/popularCategories/month/" + month).Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                categories = JsonConvert.DeserializeObject<List<PopularCategory>>(data);
            }
            return View(categories);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            OrderViewModel order = new OrderViewModel();
            HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Orders/" + id).Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                order = JsonConvert.DeserializeObject<OrderViewModel>(data);
            }
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirm(int id)
        {
            try
            {
                HttpResponseMessage respone = _client.DeleteAsync(_client.BaseAddress + "/Orders/" + id).Result;
                if (respone.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Order Deleted Successfully!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();
        }

    }
}
