using DTO.Models;
using Microsoft.AspNetCore.Mvc;
//using MVCModel.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;

namespace MVCModel.Controllers
{
    public class BookController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7160/api");
        private readonly HttpClient _client;

        public BookController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        #region GET ALL BOOKS
        [HttpGet]
        public IActionResult Index()
        {
            List<BookModel> books = new List<BookModel>();
            HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Books/all").Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                books = JsonConvert.DeserializeObject<List<BookModel>>(data);
            }
            return View(books);
        }
        #endregion

        #region DETAIL
        // DETAIL
        [HttpGet]
        public IActionResult Detail(int id)
        {
            BookModel book = new BookModel();
            HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Books/" + id).Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                book = JsonConvert.DeserializeObject<BookModel>(data);
            }
            return View(book);
        }
        #endregion

        #region LINQ DEMO
        // GET BOOKS WITH PAGGING
        [HttpGet]
        public IActionResult BookWithPaging(int pageIndex)
        {
            List<LinQBookModel> books = new List<LinQBookModel>();
            HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Books/page/" + pageIndex).Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                books = JsonConvert.DeserializeObject<List<LinQBookModel>>(data);
            }
            return View(books);
        }

        // GET FILTERED CATEGOBY BY PRICE
        [HttpGet]
        public IActionResult CategoryByPrice(double price)
        {
            List<CategoryPriceModel> categoryPriceModel = new List<CategoryPriceModel>();
            HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Books/category/" + price).Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                categoryPriceModel = JsonConvert.DeserializeObject<List<CategoryPriceModel>>(data);
            }
            return View(categoryPriceModel);
        }

        //GET CATEGORY WITH BOOK DETAIL
        [HttpGet]
        public IActionResult CategoryWithBookDetail()
        {
            List<CategoryWithBookModel> categoryWithBook = new List<CategoryWithBookModel>();
            HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Books/categoryWithBooks").Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                categoryWithBook = JsonConvert.DeserializeObject<List<CategoryWithBookModel>>(data);
            }
            return View(categoryWithBook);
        }

        //GET BOOK SUMARY BY CATEGORY
        [HttpGet]
        public IActionResult BookSumaryByCategory()
        {
            List<BookSumaryModel> bookSum = new List<BookSumaryModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Books/bookSumary").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                bookSum = JsonConvert.DeserializeObject<List<BookSumaryModel>>(data);
            }
            return View(bookSum);
        }

        // GET BOOK WITH CATEGORY DETAIL
        [HttpGet]
        public IActionResult BookWithCategoryDetail()
        {
            List<LinQBookModel> bookWithCate = new List<LinQBookModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Books/bookWithCategoryDetail").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                bookWithCate = JsonConvert.DeserializeObject<List<LinQBookModel>>(data);
            }
            return View(bookWithCate);
        }

        // GET BOOKS BETWEEN 2 PRICES
        public IActionResult BookUnion()
        {
            List<BookModel> books = new List<BookModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Books/bookUnion").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                books = JsonConvert.DeserializeObject<List<BookModel>>(data);
            }
            return View(books);
        }
        #endregion

        #region CREATE
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BookModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage respone = _client.PostAsync(_client.BaseAddress + "/Books", content).Result;
                if (respone.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Product Created Successfully!";
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
        #endregion

        #region UPDATE
        [HttpGet]
        public IActionResult Edit(int id)
        {
            BookModel book = new BookModel();
            HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Books/" + id).Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                book = JsonConvert.DeserializeObject<BookModel>(data);
            }
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(BookModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage respone = _client.PutAsync(_client.BaseAddress + "/Books/" + model.Id, content).Result;
                if (respone.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Product Updated Successfully";
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
        #endregion

        #region DELETE
        [HttpGet]
        public IActionResult Delete(int id)
        {
            BookModel book = new BookModel();
            HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Books/" + id).Result;
            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                book = JsonConvert.DeserializeObject<BookModel>(data);
            }
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirm(int id)
        {
            try
            {
                HttpResponseMessage respone = _client.DeleteAsync(_client.BaseAddress + "/Books/" + id).Result;
                if (respone.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Product Deleted Successfully!";
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
        #endregion
    }
}
