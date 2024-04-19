using Microsoft.EntityFrameworkCore;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Entities.POCOs;
using OnlineStoreApp.Repository.EFCore.DataContext;

namespace OnlineStoreApp.Repository.EFCore.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task CreateCategoryAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
        }

        public async Task DeleteCategoryAsync(int IdCategory)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(f => f.Id == IdCategory);
            _dbContext.Categories.Remove(category);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<Category> GetAsync(int idCategory)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(f => f.Id == idCategory);
        }

        public void UpdateCategory(Category category)
        {
            _dbContext.Categories.Update(category);
        }
    }
}
