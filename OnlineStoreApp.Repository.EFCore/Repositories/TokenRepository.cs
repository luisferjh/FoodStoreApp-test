using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Entities.POCOs;
using OnlineStoreApp.UseCases.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineStoreApp.Repository.EFCore.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public TokenRepository(
            IOptions<JwtSettings> jwtSettings,
            IUserRepository userRepository,
            IConfiguration config)
        {
            _jwtSettings = jwtSettings.Value;
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<JwtToken> GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("JwtSettings:SecretKey")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            try
            {
                var userClaimsDB = await GetClaimsFromDB(user.Id);
                claims.AddRange(userClaimsDB
                    .Select(s => new Claim(s.ClaimType, s.ClaimValue))
                    .ToList());

                JwtSecurityToken token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.Now.Add(_jwtSettings.TokenLifeTime),
                signingCredentials: credentials);

                return new JwtToken
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    ExpireTime = token.ValidTo
                };
            }
            catch (Exception ex)
            {
                throw;
            }



        }

        public async Task<List<UserClaims>> GetClaimsFromDB(int idUser)
        {
            List<UserClaims> userClaims = await _userRepository.GetUserClaims(idUser);
            return userClaims;
        }
    }
}
