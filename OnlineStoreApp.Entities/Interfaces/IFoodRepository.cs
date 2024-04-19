using OnlineStoreApp.Entities.POCOs;

namespace OnlineStoreApp.Entities.Interfaces
{
    public interface IFoodRepository
    {
        Task<IEnumerable<Food>> GetAllAsync();
        Task<Food> GetAsync(int idFood);
        Task CreateFoodAsync(Food food);
        void UpdateFood(Food food);
        Task DeleteFoodAsync(int idFood);
        Task UpdateStockFoodAsync(int idFood, int stockSubstract);
        Task<int> GetStockFoodAsync(int idFood);
    }
}
