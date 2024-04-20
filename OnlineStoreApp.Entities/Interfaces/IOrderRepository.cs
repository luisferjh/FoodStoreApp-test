using OnlineStoreApp.Entities.POCOs;

namespace OnlineStoreApp.Entities.Interfaces
{
    public interface IOrderRepository
    {
        Task InsertOrderAsync(Order order);
        Task InsertOrderDetailAsync(OrderDetail orderDetail);
        Task<List<Order>> GetAllAsync();
        Task<Order> GetAsync(int orderId);
        Order Get(int orderId);
    }
}
