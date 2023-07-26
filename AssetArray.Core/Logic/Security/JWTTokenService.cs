using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AssetArray.Core.Logic.Security
{
    public interface IJWTTokenService
    {
        public string GenerateToken(string authKey);
    }
    public class JWTTokenService : IJWTTokenService
    {
        private readonly IConfiguration _config;

        public JWTTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(string authKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var authPassword = _config["AuthSettings:Password"];
            var expirationTime = _config.GetValue<int>("AuthSettings:ExpirationTime");
            if (string.IsNullOrEmpty(authPassword))
            {
                throw new InvalidOperationException("AuthSettings:Password is not set in the configuration");
            }

            // authKey need to validated from database here.
            if(authKey != authPassword)
            {
                return "";
            }

            var authPasswordBytes = Encoding.UTF8.GetBytes(authPassword);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity( new Claim[]
                {
                    new Claim(ClaimTypes.Role, "Admin"),
                }),
                Expires = DateTime.UtcNow.AddHours(expirationTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(authPasswordBytes), SecurityAlgorithms.HmacSha256Signature),
                Issuer= _config["AuthSettings:apiServerURL"],
                Audience = _config["AuthSettings:applicationURL"]

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string jwtToken = tokenHandler.WriteToken(token);   

            return jwtToken;
        }
    }
}
