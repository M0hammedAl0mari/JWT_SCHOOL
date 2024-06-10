using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace lastTest.Services
{
    public class TokenService
    {
        public string GenerateToken(int userId,  string roleName)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("[SECRET USED TO SIGN AND VERIFY JWT TOKENS, IT CAN BE ANYSTRING]"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> {
         new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
         new Claim(ClaimTypes.Role, roleName)
     };

            var token = new JwtSecurityToken(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
