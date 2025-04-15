using MedicalCenter.Model;

namespace MedicalCenter.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(ApplicationUser user);
    }
}
