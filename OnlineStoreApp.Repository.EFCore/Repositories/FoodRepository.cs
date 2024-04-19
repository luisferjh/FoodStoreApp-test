using Microsoft.EntityFrameworkCore;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Entities.POCOs;
using OnlineStoreApp.Repository.EFCore.DataContext;

namespace OnlineStoreApp.Repository.EFCore.Repositories
{
    public class FoodRepository : IFoodRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FoodRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task CreateFoodAsync(Food food)
        {
            await _dbContext.Foods.AddAsync(food);
        }

        public async Task DeleteFoodAsync(int idFood)
        {
            var food = await _dbContext.Foods.FirstOrDefaultAsync(f => f.Id == idFood);
            _dbContext.Foods.Remove(food);
        }

        public async Task<IEnumerable<Food>> GetAllAsync()
        {
            return await _dbContext.Foods.Include(f => f.Category).ToListAsync();
        }

        public async Task<Food> GetAsync(int idFood)
        {
            return await _dbContext.Foods.Include(f => f.Category).FirstOrDefaultAsync(f => f.Id == idFood);
        }

        public async Task<int> GetStockFoodAsync(int idFood)
        {

            var food = await _dbContext.Foods.FirstOrDefaultAsync(f => f.Id == idFood);
            return food.QuantityAvailable;

        }

        public void UpdateFood(Food food)
        {
            _dbContext.Foods.Update(food);
        }

        public async Task UpdateStockFoodAsync(int idFood, int stockSubstract)
        {
            Food food = await _dbContext.Foods.FirstOrDefaultAsync(f => f.Id == idFood);
            food.QuantityAvailable = food.QuantityAvailable - stockSubstract;
            _dbContext.Foods.Update(food);

        }
    }
}
