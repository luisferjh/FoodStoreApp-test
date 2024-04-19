using OnlineStore.DTOs;

namespace OnlineStoreApp.UseCases.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetAsync(int id);
        Task<UserDto> GetAsync(string email);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<ResultService> InsertAsync(UserCreateDTO model);
        string GetEmailUserAuth();
    }
}
