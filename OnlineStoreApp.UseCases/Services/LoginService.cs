using Microsoft.Extensions.Options;
using OnlineStore.DTOs;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Entities.POCOs;
using OnlineStoreApp.UseCases.Helpers;
using OnlineStoreApp.UseCases.Interfaces;

namespace OnlineStoreApp.UseCases.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWorkAdapter _unitOfWorkAdapter;
        private readonly ITokenService _tokenService;

        public LoginService(
            IUnitOfWorkAdapter unitOfWorkAdapter,
            ITokenService tokenService,
            IOptions<KeyValuesConfiguration> keyValuesConfiguration)
        {
            _unitOfWorkAdapter = unitOfWorkAdapter;
            _tokenService = tokenService;
            SecurityTools._keyValuesConfiguration = keyValuesConfiguration.Value;
        }

        public async Task<AuthenticationResultDTO> LoginAsync(LoginDTO loginUser)
        {
            (User user, AuthenticationResultDTO result) = await AuthenticateAsync(loginUser);

            if (user == null)
                return result;

            JwtToken token = await _tokenService.GenerateToken(user);

            result.Result = token;
            result.Success = true;

            return result;
        }

        public async Task<(User, AuthenticationResultDTO)> AuthenticateAsync(LoginDTO loginDTO)
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            User user = await _unitOfWork.UnitOfWorkRepositories.UserRepository.Get(loginDTO.Email);
            AuthenticationResultDTO result = new AuthenticationResultDTO();
            if (user == null)
            {
                result.Success = false;
                result.Errors.Add("User does not exist");
                return (null, result);
            }

            if (!CheckPassword(user.Password, loginDTO.Password))
            {
                result.Success = false;
                result.Errors.Add("Password combination failed");
                return (null, result);
            }

            return (user, result);
        }

        private bool CheckPassword(string passwordStore, string passwordSent)
           => SecurityTools.DecryptString(passwordStore) == passwordSent;

    }
}
