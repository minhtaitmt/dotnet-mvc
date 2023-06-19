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

    }
}
