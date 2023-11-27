using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UserBookToken.Token
{
    public class TokenGenerator
    {


        public string GenerateToken(string email,string id,string role) 
        {


            var myclaims = new List<Claim>() 
            {
               new Claim(ClaimTypes.Role,role),
               new Claim(ClaimTypes.Email,email),
               new Claim(ClaimTypes.NameIdentifier,id)
                


            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ozan16926ozan16926ozan16926ozan19"));
            SigningCredentials signingCredentials = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "http://localhost",
                audience: "http://localhost",
                notBefore: DateTime.Now,
                expires : DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials,
                claims: myclaims );

            return handler.WriteToken(token);





        }
    }
}
