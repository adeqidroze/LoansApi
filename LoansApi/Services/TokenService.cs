using DataBaseContext.Data;
using LoansApi.DTOs;
using LoansApi.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace LoansApi.Services
{
    public interface ITokenInterface
    {
        public string GenerateToken(UserLogin userCreds);
        public ClaimsPrincipal GetPrincipal(string token);
        public string ValidateToken(string token);
    }
    public class TokenService : ITokenInterface
    {
        private readonly AppSettings _appSettings;
        private readonly LoanDatabaseContext _context;
        public TokenService(IOptions<AppSettings> set, LoanDatabaseContext context)
        {
            _appSettings = set.Value;
            _context = context;
        }

        public string GenerateToken(UserLogin userCreds)
        {
            var currentUser = _context.Users.Where(x => x.UserName == userCreds.Username).SingleOrDefault();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, currentUser.Id.ToString()),
                new Claim(ClaimTypes.Name, currentUser.UserRole.ToString()),
                new Claim(ClaimTypes.Name, userCreds.Username.ToString())

                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null) return null;
                byte[] key = Convert.FromBase64String(_appSettings.Secret);
                var  parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken securityToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }
        public string ValidateToken(string token)
        {
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null) 
                return null;

            ClaimsIdentity identity;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            string username = usernameClaim.Value;
            return username;
        }
    }
}
