using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserBookToken.Entities;

namespace UserBookToken.Token
{
    public class TokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public TokenGenerator(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public string GenerateToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("Token:Key").Value);
            var userRole = _userManager.GetRolesAsync(user).Result;
            List<Claim> claims = userRole.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Id),
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Role,userRole[0]),
                    new Claim(ClaimTypes.DateOfBirth,user.BirthDate.ToString() ?? string.Empty),
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _configuration.GetSection("Token:Audience").Value,
                Issuer = _configuration.GetSection("Token:Issuer").Value,
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddDays(1),
            };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                 return tokenHandler.WriteToken(token);
        }
    }
}


