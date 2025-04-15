using Microsoft.EntityFrameworkCore;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Entities.POCOs;
using OnlineStoreApp.Repository.EFCore.DataContext;

namespace OnlineStoreApp.Repository.EFCore.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task InsertOrderAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
        }

        public async Task InsertOrderDetailAsync(OrderDetail orderDetail)
        {
            await _dbContext.OrderDetails.AddAsync(orderDetail);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _dbContext.Orders
                .Include(f => f.OrderDetails)
                .ThenInclude(f => f.Food)
                .ToListAsync();
        }

        public async Task<Order> GetAsync(Guid orderId)
        {
            return await _dbContext.Orders
               .Where(f => f.Id == orderId)
               .Include(f => f.OrderDetails)
               .ThenInclude(f => f.Food)
               .FirstOrDefaultAsync();
        }

        public Order Get(Guid orderId)
        {
            return _dbContext.Orders
                .Where(f => f.Id == orderId)
                .Include(f => f.OrderDetails)
                .ThenInclude(f => f.Food)
                .FirstOrDefault();
        }
    }
}
