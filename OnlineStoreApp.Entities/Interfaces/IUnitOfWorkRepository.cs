namespace OnlineStoreApp.Entities.Interfaces
{
    public interface IUnitOfWorkRepository
    {
        ICategoryRepository CategoryRepository { get; }
        IFoodRepository FoodRepository { get; }
        IOrderRepository OrderRepository { get; }
        IUserRepository UserRepository { get; }
        ITokenRepository tokenRepository { get; }

        Task<bool> SaveAsync();
        bool Save();
    }
}
