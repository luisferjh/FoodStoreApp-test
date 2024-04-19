using OnlineStoreApp.Entities.POCOs;

namespace OnlineStoreApp.UseCases.Interfaces
{
    public interface ITokenService
    {
        Task<JwtToken> GenerateToken(User user);
        Task<List<UserClaims>> GetClaimsFromDB(int idUser);
    }
}
