using GenericRepositoryAndUnitofWork.Entities;
using GenericRepositoryAndUnitofWork.Models;

namespace GenericRepositoryAndUnitofWork.Repositories
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders();
        Order GetOrderById(int id);
        List<OrderDetail> GetOrderDetailsById(int id);
        Task AddOrder(Order order);
        void UpdateOrder(int id, Order order);
        void DeleteOrder(int id);

        List<BestSellerModel> GetTenBestSellerBooks(int month);
        List<BestCategoryModel> GetBestSellerCategory(int month);
        TotalBookAndCategorySell GetTotalBookAndCategorySell(int month);
        List<UnPopularBooks> GetUnpopularBooks(int month);
        Book GetBestSellerBook(int month);
    }
}

