using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using FYPIBDPatientApp.Dtos;
using FYPIBDPatientApp.Data;
using FYPIBDPatientApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FYPIBDPatientApp.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenService(IConfiguration configuration, AppDbContext context, UserManager<ApplicationUser> userManager) 
        {
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
        }

        public TokenResultDto CreateToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessTokenExpiry = DateTime.Now.AddMinutes(30);

            var refreshTokenExpiry = DateTime.Now.AddDays(7);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: accessTokenExpiry,
                signingCredentials: creds);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            var refreshToken = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();


            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                Expires = refreshTokenExpiry,
                Created = DateTime.Now,
                isRevoked = false
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            _context.SaveChangesAsync();

            return new TokenResultDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expires = accessTokenExpiry
            };
        }

        public async Task<TokenResultDto> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (storedToken == null || storedToken.Expires < DateTime.Now || storedToken.isRevoked)
            {
                throw new Exception("Invalid or expired refresh token.");
            }

            storedToken.isRevoked = true;
            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(storedToken.UserId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);
    
            return CreateToken(user, roles);
        }

    }
}
