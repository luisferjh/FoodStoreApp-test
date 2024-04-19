using Microsoft.EntityFrameworkCore;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Entities.POCOs;
using OnlineStoreApp.Repository.EFCore.DataContext;

namespace OnlineStoreApp.Repository.EFCore.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User model)
        {
            _dbContext.Users.Add(model);
        }

        public void Deactivate(User user)
        {
            user.State = 0;
            _dbContext.Users.Update(user);
        }

        public async Task<User> Get(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(f => f.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(f => f.Id == id);
            return user;
        }

        public async Task<List<UserClaims>> GetUserClaims(int idUser)
        {
            return await _dbContext.UserClaims.Where(f => f.UserId == idUser).ToListAsync();
        }
    }
}
