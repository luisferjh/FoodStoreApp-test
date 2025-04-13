namespace OnlineStoreApp.Entities.Interfaces
{
    public interface IUnitOfWorkRepositories
    {
        ICategoryRepository CategoryRepository { get; }
        IFoodRepository FoodRepository { get; }
        IOrderRepository OrderRepository { get; }
        IUserRepository UserRepository { get; }
        ITokenRepository TokenRepository { get; }
    }
}
