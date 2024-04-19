using OnlineStore.DTOs;
using OnlineStoreApp.Entities.POCOs;

namespace OnlineStoreApp.UseCases.Interfaces
{
    public interface ILoginService
    {
        Task<AuthenticationResultDTO> LoginAsync(LoginDTO loginUser);
        Task<(User, AuthenticationResultDTO)> AuthenticateAsync(LoginDTO loginDTO);
    }
}
