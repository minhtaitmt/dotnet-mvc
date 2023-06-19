using AutoMapper;
using GenericRepositoryAndUnitofWork.Entities;
using GenericRepositoryAndUnitofWork.Models;

namespace GenericRepositoryAndUnitofWork.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() 
        { 
            CreateMap<Book, BookModel>().ReverseMap();
            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailInputModel>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailModel>().ReverseMap();
            CreateMap<Order, OrderViewModel>().ReverseMap();
            CreateMap<Order, OrderModel>().ReverseMap();
        }
    }
}
