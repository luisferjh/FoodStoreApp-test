using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Entities.POCOs;
using OnlineStoreApp.UseCases.Interfaces;

namespace OnlineStoreApp.UseCases.Services
{
    public class TokenService : ITokenService
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public TokenService(
            IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<JwtToken> GenerateToken(User user)
        {
            return await _unitOfWork.tokenRepository.GenerateToken(user);
        }

        public async Task<List<UserClaims>> GetClaimsFromDB(int idUser)
        {
            return await _unitOfWork.tokenRepository.GetClaimsFromDB(idUser);
        }
    }
}
