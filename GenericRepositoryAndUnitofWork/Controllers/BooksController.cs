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
    public class BooksController : ControllerBase
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IMapper _mapper;

        public BooksController(IUnitofWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public IActionResult GetAllBook()
        {
            var books = _unitOfWork.BookRepository.GetAllBooks();
            if (books == null)
            {
                return NotFound();
            }
            var booksModel = _mapper.Map<List<BookModel>>(books);
            return Ok(booksModel);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetBookById(int id)
        //{
        //    var book = await _unitOfWork.BookRepository.GetBookByIdAsync(id);
        //    if (book == null)
        //    {
        //        return NotFound();
        //    }
        //    var bookModel = _mapper.Map<BookModel>(book);
        //    return Ok(bookModel);
        //}

        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _unitOfWork.BookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            var bookModel = _mapper.Map<BookModel>(book);
            return Ok(bookModel);
        }

        //[HttpGet("search")]
        //public async Task<IActionResult> GetBooksWithLinq(string? name, double? priceFrom, double? priceTo, string? sortBy, int pageIndex = 1)
        //{
        //    var booksModel = await _unitOfWork.BookRepository.GetBooksWithLinqAsync(name, priceFrom, priceTo, sortBy, pageIndex);
        //    if (booksModel == null)
        //    {
        //        return BadRequest("Can not get books!");
        //    }
        //    return Ok(booksModel);
        //}

        [HttpGet("page/{index}")]
        public IActionResult GetBooksWithPaging(int index)
        {
            var bookModel = _unitOfWork.BookRepository.GetBooksWithPaging(index);
            if(bookModel == null)
            {
                return BadRequest("Can not get books!");
            }
            return Ok(bookModel);
        }

        // Get Categories which contain Books with Book Price > price | DISTINCT || DONE
        [HttpGet("category/{price}")]
        public IActionResult GetCategoryByPrice(double price)
        {
            var cateWithPriceModel = _unitOfWork.BookRepository.GetCategoryByPrice(price);
            if (cateWithPriceModel == null)
            {
                return BadRequest("Can not get categories!");
            }
            return Ok(cateWithPriceModel);
        }

        // Get Book sumary by categoryId || DONE
        [HttpGet("bookSumary")]
        public IActionResult GetBookSumaryByCategory()
        {
            var bookModel = _unitOfWork.BookRepository.GetBookSumaryByCategory();
            if (bookModel == null)
            {
                return NotFound();
            }
            return Ok(bookModel);
        }

        //Get Books with Category Detail | Join || DONE
        [HttpGet("bookWithCategoryDetail")]
        public IActionResult GetBookWithCategoryDetail()
        {
            var bookModel = _unitOfWork.BookRepository.GetBookWithCategoryDetail();
            if (bookModel == null)
            {
                return NotFound();
            }
            return Ok(bookModel);
        }


        //Get all Category both contain and not contan Book | Left Join || DONE
        [HttpGet("categoryWithBooks")]
        public IActionResult GetCategoryWithBook()
        {
            var bookModel = _unitOfWork.BookRepository.GetCategoryWithBook();
            if (bookModel == null)
            {
                return NotFound();
            }
            return Ok(bookModel);
        }


        // Get Books between 2 prices | Union
        [HttpGet("bookUnion")]
        public IActionResult GetBookUnion()
        {
            var bookModel = _unitOfWork.BookRepository.GetBookUnion();
            if (bookModel == null)
            {
                return NotFound();
            }
            return Ok(bookModel);
        }

        //[HttpPost]
        //public IActionResult AddBook(BookModel bookModel)
        //{
        //    var book = _mapper.Map<Book>(bookModel);
        //    try
        //    {
        //        _unitOfWork.BookRepository.AddBook(book);
        //        _unitOfWork.SaveChanges();
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //    return CreatedAtAction("GetBookById", new { id = book.Id }, _mapper.Map<BookModel>(book));
        //}

        [AuthorizeFilter("admin")]
        [HttpPost]
        public IActionResult AddBook(BookModel bookModel)
        {
            var book = _mapper.Map<Book>(bookModel);
            using (var _trans = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _unitOfWork.BookRepository.AddBook(book);
                    _unitOfWork.SaveChanges();
                    _trans.Commit();
                }catch (Exception err) 
                {
                    _trans.Rollback();
                    return BadRequest(err);
                }
            }
                
            return CreatedAtAction("GetBookById", new { id = book.Id }, _mapper.Map<BookModel>(book));
        }

        [AuthorizeFilter("admin")
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, BookModel bookModel)
        {
            var book = _mapper.Map<Book>(bookModel);

            using (var _trans = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _unitOfWork.BookRepository.UpdateBook(id,book);
                    _unitOfWork.SaveChanges();
                    _trans.Commit();
                }
                catch (Exception err)
                {
                    _trans.Rollback();
                    return BadRequest(err);
                }
            }
            return NoContent();
        }

        [AuthorizeFilter("admin", "hr")]
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            using (var _trans = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _unitOfWork.BookRepository.DeleteBook(id);
                    _unitOfWork.SaveChanges();
                    _trans.Commit();
                }
                catch (Exception err)
                {
                    _trans.Rollback();
                    return BadRequest(err);
                }
            }
            return NoContent();
        }

        [AuthorizeFilter("admin", "hr")]
        [HttpPut("UpdatePrices")]
        public async Task<IActionResult> UpdateBookPrices(List<int> bookIds, double newPrice)
        {
            try
            {
                List<Book> updatedBooks = await _unitOfWork.BookRepository.UpdateBookPricesAsync(bookIds, newPrice);
                _unitOfWork.SaveChanges();
                return Ok(updatedBooks);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi, trả về lỗi nếu có
                return StatusCode(500, ex.Message);
            }
        }

    }
}
