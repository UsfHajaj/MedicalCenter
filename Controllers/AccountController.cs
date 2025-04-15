using MedicalCenter.Data.DTOs;
using MedicalCenter.Model;
using MedicalCenter.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MedicalCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;

        public AccountController(UserManager<ApplicationUser> userManager,IConfiguration configuration,IJwtService jwtService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _jwtService = jwtService;
        }
        [HttpPost("register/user")]
        public async Task<IActionResult> Register(RegisterUserDTO UserFromRequest)
        {
            if (ModelState.IsValid)
            {
                Patient UserFromDb = new Patient();
                UserFromDb.UserName = UserFromRequest.UserName;
                UserFromDb.Email = UserFromRequest.Email;

                IdentityResult result = await _userManager.CreateAsync(UserFromDb, UserFromRequest.Password);
                if(result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(UserFromDb, "user");
                    return Ok(new { message = "Account created successfully with role." });

                }
                return BadRequest(result.Errors.FirstOrDefault().Description.ToString());
            }
            return BadRequest(ModelState);
            
        }
        [HttpPost("register/Admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterUserDTO UserFromRequest)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser UserFromDb = new ApplicationUser();
                UserFromDb.UserName = UserFromRequest.UserName;
                UserFromDb.Email = UserFromRequest.Email;

                IdentityResult result = await _userManager.CreateAsync(UserFromDb, UserFromRequest.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(UserFromDb, "admin");
                    return Ok(new { message = "Account created successfully with role." });

                }
                return BadRequest(result.Errors.FirstOrDefault().Description.ToString());
            }
            return BadRequest(ModelState);

        }
        [HttpPost("register/Doctor")]
        public async Task<IActionResult> RegisterDoctor(RegisterUserDTO UserFromRequest)
        {
            if (ModelState.IsValid)
            {
                Doctor UserFromDb = new Doctor();
                UserFromDb.UserName = UserFromRequest.UserName;
                UserFromDb.Email = UserFromRequest.Email;

                IdentityResult result = await _userManager.CreateAsync(UserFromDb, UserFromRequest.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(UserFromDb, "doctor");
                    return Ok(new { message = "Account created successfully with role." });

                }
                return BadRequest(result.Errors.FirstOrDefault().Description.ToString());
            }
            return BadRequest(ModelState);

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto UserFromRequest)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(UserFromRequest.Email);
                if (user != null)
                {
                    ApplicationUser userFromDb = new ApplicationUser();
                    userFromDb.Email = UserFromRequest.Email;
                    var result = await _userManager.CheckPasswordAsync(user, UserFromRequest.Password);

                    if (result)
                    {
                        var tokenGenerated = _jwtService.GenerateJwtToken(user);
                        return Ok(new 
                        {
                            token=tokenGenerated,
                            expiration=DateTime.Now.AddDays(1),
                        });
                    }
                }
                return Unauthorized();
            }
            return BadRequest(ModelState);
        }


    }
}
