using Microsoft.AspNetCore.Authentication;

namespace MedicalCenter.Services.Interfaces
{
    public interface IGoogleService
    {
        AuthenticationProperties GetGoogleLoginProperties(string redirectUri);
        Task<string> GoogleLoginCallbackAsync();
    }
}
