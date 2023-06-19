using AutoMapper;
using GenericRepositoryAndUnitofWork.Entities;
using GenericRepositoryAndUnitofWork.Models;
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

        [HttpPost]
        public async Task<IActionResult> AddOrder(OrderModel orderModel)
        {
            var order = _mapper.Map<Order>(orderModel);
            try
            {
                await _unitOfWork.OrderRepository.AddOrder(order);
                _unitOfWork.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }
            return CreatedAtAction("GetOrderById", new { id = order.Id }, order);
        }

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
        public IActionResult GetTenBestSellerBooks(int month)
        {
            var orders = _unitOfWork.OrderRepository.GetTenBestSellerBooks(month);
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
        public IActionResult GetUnpopularBooks(int month)
        {
            var books = _unitOfWork.OrderRepository.GetUnpopularBooks(month);
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

    }
}
