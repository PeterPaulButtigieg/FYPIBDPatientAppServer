using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FYPIBDPatientApp.Services;
using FYPIBDPatientApp.Dtos;
using FYPIBDPatientApp.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using FYPIBDPatientApp.Data;

namespace FYPIBDPatientApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly AppDbContext _context;

        public AuthController(UserManager<ApplicationUser> userManager, 
               SignInManager<ApplicationUser> signInManager, 
               TokenService tokenService,
               AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) 
            {
                return Unauthorized("Invalid credentials.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var tokenResult = _tokenService.CreateToken(user, roles);

            return Ok(tokenResult); // e.g. { accessToken, refreshToken, expires }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutDto request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest("Refresh token is required for logout.");
            }

            // Find the refresh token in the database.
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);
            if (storedToken == null)
            {
                return BadRequest("Invalid refresh token.");
            }

            // Revoke the refresh token.
            storedToken.isRevoked = true;
            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();

            return Ok("Logged out successfully.");
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest("Refresh token is required.");
            }

            try
            {
                var tokenResult = await _tokenService.RefreshTokenAsync(request.RefreshToken);
                return Ok(tokenResult);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser 
            { 
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                MobileNumber = model.MobileNumber,

            };

            var createResult = await _userManager.CreateAsync(user, model.Password);
            if (!createResult.Succeeded) 
            {
                return BadRequest(createResult.Errors);
            }

            await _userManager.AddToRoleAsync(user, "Patient");

            return Ok("User registered successfully.");
        }
    }
}
