using AutoMapper;
using DTO.Models;
using GenericRepositoryAndUnitofWork.Entities;
using GenericRepositoryAndUnitofWork.Filters;
//using GenericRepositoryAndUnitofWork.Models;
using GenericRepositoryAndUnitofWork.UnitofWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenericRepositoryAndUnitofWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrdersController(IUnitofWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [AuthorizeFilter("admin")]
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var orders = _unitOfWork.OrderRepository.GetAllOrders();
            if (orders == null)
            {
                return NotFound();
            }
            var orderModel = _mapper.Map<List<OrderViewModel>>(orders);
            return Ok(orderModel);
        }

        [AuthorizeFilter("admin")]
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var order = _unitOfWork.OrderRepository.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            var orderModel = _mapper.Map<OrderViewModel>(order);
            return Ok(orderModel);
        }

        [AuthorizeFilter("admin")]
        [HttpGet("details/{id}")]
        public IActionResult GetOrderDetailsById(int id)
        {
            var details = _unitOfWork.OrderRepository.GetOrderDetailsById(id);
            if (details == null)
            {
                return NotFound();
            }
            var detailsModel = _mapper.Map<List<OrderDetailModel>>(details);
            return Ok(detailsModel);
        }

        [AuthorizeFilter("admin")]
        [HttpPost]
        public async Task<IActionResult> AddOrder(OrderModel orderModel)
        {
            var order = _mapper.Map<Order>(orderModel);
            try
            {
                await _unitOfWork.OrderRepository.AddOrder(order);
                //_unitOfWork.SaveChanges();
            }
            catch
            {
                return BadRequest("Fail to add order!");
            }
            return CreatedAtAction("GetOrderById", new { id = order.Id }, order);
        }

        [AuthorizeFilter("admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            try
            {
                _unitOfWork.OrderRepository.DeleteOrder(id);
                _unitOfWork.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }
            return NoContent();
        }

        /////////////////////////////////////////////////////////////////////
        [HttpGet("bestSeller/month/{month}")]
        public async Task<IActionResult> GetTenBestSellerBooks(int month)
        {
            var orders = await _unitOfWork.OrderRepository.GetTenBestSellerBooksAsync(month);
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpGet("bestSellerCate/month/{month}")]
        public IActionResult GetBestSellerCategory(int month)
        {
            var orders = _unitOfWork.OrderRepository.GetBestSellerCategory(month);
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpGet("TotalBookAndCategory/month/{month}")]
        public IActionResult GetTotalBookAndCategorySell(int month)
        {
            var totalModel = _unitOfWork.OrderRepository.GetTotalBookAndCategorySell(month);
            if (totalModel == null)
            {
                return NotFound();
            }
            return Ok(totalModel);
        }

        [HttpGet("UnPopularBook/month/{month}")]
        public async Task<IActionResult> GetUnpopularBooks(int month)
        {
            var books = await _unitOfWork.OrderRepository.GetUnpopularBooks(month);
            if (books == null)
            {
                return NotFound();
            }
            return Ok(books);
        }

        [HttpGet("BestSellerBook/month/{month}")]
        public IActionResult GetBestSellerBook(int month)
        {
            var book = _unitOfWork.OrderRepository.GetBestSellerBook(month);
            if (book == null)
            {
                return NotFound();
            }
            var bookModel = _mapper.Map<BookModel>(book);
            return Ok(bookModel);
        }

        [HttpGet("popularCategories/month/{month}")]
        public async Task<IActionResult> GetPopularCategories(int month)
        {
            var categories = await _unitOfWork.OrderRepository.GetPopularCategoriesAsync(month);
            if (categories == null)
            {
                return NotFound();
            }
            return Ok(categories);
        }

        [HttpGet("highestRevenueBooks/month/{month}")]
        public async Task<IActionResult> GetHighestRevenueBooks(int month)
        {
            var books = await _unitOfWork.OrderRepository.GetHighestRevenueBooksAsync(month);
            if (books == null)
            {
                return NotFound();
            }
            return Ok(books);
        }

        [HttpGet("monthlyRevenue/year/{year}")]
        public async Task<IActionResult> GetMonthlyRevenue(int year)
        {
            var revenue = await _unitOfWork.OrderRepository.GetMonthlyRevenueAsync(year);
            if (revenue == null)
            {
                return NotFound();
            }
            return Ok(revenue);
        }



    }
}
