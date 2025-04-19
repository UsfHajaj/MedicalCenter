using MedicalCenter.Data.DTOs;
using MedicalCenter.Model;
using MedicalCenter.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
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
        private readonly IGoogleService _googleService;

        public AccountController(UserManager<ApplicationUser> userManager,IConfiguration configuration,IJwtService jwtService,IGoogleService googleService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _jwtService = jwtService;
            _googleService = googleService;
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

        [HttpGet("LoginWithGoogle")]
        public IActionResult LoginWithGoogle()
        {
            var properties= _googleService.GetGoogleLoginProperties(Url.Action(nameof(GoogleLoginCallback)));
            return Challenge(properties, "Google"); 
        }
        [HttpGet("GoogleLoginCallback")]
        public async Task<IActionResult> GoogleLoginCallback()
        {
            try
            {
                var token =await _googleService.GoogleLoginCallbackAsync();
                return Redirect($"http://localhost:4200/auth/login?token={token}");
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("External login failed.");
            }
            catch
            {
                return BadRequest("An error occurred during Google login.");
            }
        }



        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) return BadRequest("Email confirmation failed");

            var confirmtionLinkForFront = $"http://localhost:4200/auth/confirm-email?userId={userId}&token={token}";
            // return Redirect(confirmtionLinkForFront);
            return Ok(new { Message = "Email confirmed successfully." });
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;


            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("Profile updated successfully");
        }

        [HttpPost("resend-email-confirmation")]
        public async Task<IActionResult> ResendEmailConfirmation([FromBody] ResendEmailConfirmationDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return NotFound("User not found");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, token }, Request.Scheme);

            // Assume a method for sending email exists
            //await SendEmailAsync(user.Email, "Confirm your email", confirmationLink);

            return Ok("Email confirmation link sent");
        }

        [HttpGet("reset-password")]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return BadRequest(new { Status = "Error", Message = "Invalid password reset link." });
            }

            // Return success response for valid tokens
            //return Ok(new { Status = "Success", Message = "Password reset link is valid.", Token = token, Email = email }); 
            return Redirect($"http://localhost:4200/auth/reset-password?token={token}&email={email}");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
                if (user == null)
                {
                    return BadRequest(new { message = "Invalid request." });
                }

                var decodedToken = WebUtility.UrlDecode(resetPasswordDto.Token);
                var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);

                if (result.Succeeded)
                {
                    return Ok(new { message = "Password has been reset successfully." });
                }

                Console.WriteLine($"Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("Password changed successfully");
        }

        [HttpGet("user-details")]
        public async Task<IActionResult> GetUserDetails()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return NotFound("User not found");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found");
            return Ok(new
            {
                user.Email,
                user.UserName,
                user.Address,
                user.PhoneNumber
            });
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user=await _userManager.FindByIdAsync(userId);
            if (user != null) return NotFound("User not found");

            var result = await _userManager.DeleteAsync(user);
            if(!result.Succeeded)
                return BadRequest(result.Errors.FirstOrDefault()!.Description.ToString());

            return Ok("Account deleted successfully");

        }




    }
}
