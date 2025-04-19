using MedicalCenter.Model;
using MedicalCenter.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedicalCenter.Services.impelementation
{
    public class GoogleService : IGoogleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtService _jwtService;

        public GoogleService(UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor, IJwtService jwtService)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _jwtService = jwtService;
        }

        public AuthenticationProperties GetGoogleLoginProperties(string redirectUri)
        {
            throw new NotImplementedException();
        }

        public Task<string> GoogleLoginCallbackAsync()
        {
            throw new NotImplementedException();
        }

        private async Task<AuthenticateResult> AuthenticateExternalUserAsync()
        {
            return await _httpContextAccessor.HttpContext!
                .AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private async Task<ApplicationUser> FindOrCreateUserAsync(ClaimsPrincipal externalUser, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user !=null) return user;
            var userName = GenerateUniqueUserName(externalUser);
            user=new ApplicationUser
            {
                UserName = userName,
                Email = email
            };
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("User creation failed");
            }
             await _userManager.AddLoginAsync(user, new UserLoginInfo("Google", externalUser.FindFirstValue(ClaimTypes.NameIdentifier), "Google"));
            return user;
        }

        private string GenerateUniqueUserName(ClaimsPrincipal externalUser)
        {
            var baseName = externalUser.FindFirstValue(ClaimTypes.Name)?.Replace(" ","_")??"User";
            return $"{baseName}_{Guid.NewGuid().ToString().Substring(0, 4)}";
        }
    }
}
