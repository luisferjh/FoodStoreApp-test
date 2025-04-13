using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Entities.POCOs;
using OnlineStoreApp.UseCases.Interfaces;

namespace OnlineStoreApp.UseCases.Services
{
    public class TokenService : ITokenService
    {
        private readonly IUnitOfWorkAdapter _unitOfWorkAdapter;

        public TokenService(
            IUnitOfWorkAdapter unitOfWorkAdapter)
        {
            _unitOfWorkAdapter = unitOfWorkAdapter;
        }

        public async Task<JwtToken> GenerateToken(User user)
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            return await _unitOfWork.UnitOfWorkRepositories.TokenRepository.GenerateToken(user);
        }

        public async Task<List<UserClaims>> GetClaimsFromDB(int idUser)
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            return await _unitOfWork.UnitOfWorkRepositories.TokenRepository.GetClaimsFromDB(idUser);
        }
    }
}
