using OnlineStoreApp.Entities.POCOs;

namespace OnlineStoreApp.Entities.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User model);
        Task AddUserClaimsAsync(List<UserClaims> models);
        Task<User> Get(string email);
        void Deactivate(User user);
        Task<List<UserClaims>> GetUserClaims(int idUser);
    }
}
