using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FYPIBDPatientApp.Services;

namespace FYPIBDPatientApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly TokenService _tokenService;

        public AuthController(UserManager<IdentityUser> userManager, 
               SignInManager<IdentityUser> signInManager, 
               TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Models.LoginDto model)
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

            // to get roles assigned to the user
            var roles = await _userManager.GetRolesAsync(user);

            // to generate the token
            var token = _tokenService.CreateToken(user, roles);
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Models.RegisterDto model)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var user = new IdentityUser 
            { 
                UserName = model.Email,
                Email = model.Email
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);
            if (!createResult.Succeeded) 
            {
                return BadRequest(createResult.Errors);
            }

            await _userManager.AddToRoleAsync(user, "User");

            return Ok("User registered successfully.");
        }
    }
}
