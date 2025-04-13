using OnlineStoreApp.Entities.Interfaces;

namespace OnlineStoreApp.Repository.EFCore.Repositories
{
    public class UnitOfWorkRepositories : IUnitOfWorkRepositories
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFoodRepository _foodRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;

        public UnitOfWorkRepositories(
            ICategoryRepository categoryRepository,
            IFoodRepository foodRepository,
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            ITokenRepository tokenRepository)
        {
            _categoryRepository = categoryRepository;
            _foodRepository = foodRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
        }

        public ICategoryRepository CategoryRepository => _categoryRepository;

        public IFoodRepository FoodRepository => _foodRepository;

        public IOrderRepository OrderRepository => _orderRepository;

        public IUserRepository UserRepository => _userRepository;

        public ITokenRepository TokenRepository => _tokenRepository;
    }
}
