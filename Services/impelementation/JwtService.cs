using MedicalCenter.Model;
using MedicalCenter.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MedicalCenter.Services.impelementation
{
    public class JwtService : IJwtService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public JwtService(UserManager<ApplicationUser> userManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public string GenerateJwtToken(ApplicationUser user)
        {
            ValidateUser(user);
            var claims = GetClaimsForUser(user);
            var signingCredentials = GetSigningCredentials();
            return CreateJwtToken(claims, signingCredentials);
        }

        private void ValidateUser(ApplicationUser user)
        {
            if(user == null)
                throw new ArgumentNullException(nameof(user));
            if(string.IsNullOrEmpty(user.Id))
                throw new ArgumentNullException(nameof(user.Id), "User Id cannot be null or empty");
            if (string.IsNullOrEmpty(user.UserName))
                throw new ArgumentNullException(nameof(user.UserName), "User Name cannot be null or empty");
            if (string.IsNullOrEmpty(user.Email))
                throw new ArgumentNullException(nameof(user.Email), "User Email cannot be null or empty");
        } 

        private List<Claim> GetClaimsForUser(ApplicationUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };
            var Roles=_userManager.GetRolesAsync(user).Result;
            foreach(var i in Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, i));
            }
            return claims;
        } 

        private SigningCredentials GetSigningCredentials()
        {
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecritKey"]));
            return new SigningCredentials(Key,SecurityAlgorithms.HmacSha256);
        }

        private string CreateJwtToken(IEnumerable<Claim> claims,SigningCredentials signingCredentials)
        {
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:IssuerIp"],
                audience: _configuration["JWT:AudienceIP"],
                claims:claims,
                expires:DateTime.UtcNow.AddDays(1),
                signingCredentials:signingCredentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
