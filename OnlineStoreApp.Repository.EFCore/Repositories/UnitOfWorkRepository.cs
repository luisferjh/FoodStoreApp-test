using Microsoft.EntityFrameworkCore;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Repository.EFCore.DataContext;

namespace OnlineStoreApp.Repository.EFCore.Repositories
{
    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFoodRepository _foodRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;

        public UnitOfWorkRepository(
            ApplicationDbContext applicationDbContext,
            ICategoryRepository categoryRepository,
            IFoodRepository foodRepository,
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            ITokenRepository tokenRepository)
        {
            _dbContext = applicationDbContext;
            _categoryRepository = categoryRepository;
            _foodRepository = foodRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
        }

        public ICategoryRepository CategoryRepository { get => _categoryRepository; }
        public IFoodRepository FoodRepository { get => _foodRepository; }
        public IOrderRepository OrderRepository { get => _orderRepository; }

        public IUserRepository UserRepository { get => _userRepository; }

        public ITokenRepository tokenRepository { get => _tokenRepository; }

        public async Task<bool> SaveAsync()
        {
            try
            {
                int result = await _dbContext.SaveChangesAsync();
                return result <= 0 ? false : true;
            }
            catch (DbUpdateException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Save()
        {
            try
            {
                int result = _dbContext.SaveChanges();
                return result <= 0 ? false : true;
            }
            catch (DbUpdateException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _dbContext.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
