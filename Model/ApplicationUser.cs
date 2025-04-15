using Microsoft.AspNetCore.Identity;

namespace MedicalCenter.Model
{
    public class ApplicationUser:IdentityUser
    {
        public string? Address { get; set; }
    }
}
