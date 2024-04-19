using OnlineStoreApp.Entities.POCOs;

namespace OnlineStoreApp.Entities.Interfaces
{
    public interface ITokenRepository
    {
        Task<JwtToken> GenerateToken(User user);
        Task<List<UserClaims>> GetClaimsFromDB(int idUser);
    }
}
