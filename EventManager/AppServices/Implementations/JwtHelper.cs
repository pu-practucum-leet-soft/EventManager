using EventManager.AppServices.Interfaces;
using EventManager.Data.Contexts;
using EventManager.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtHelper : IJwtHelper
{
    private readonly IConfiguration _config;
    //private readonly UserManager<User> _userManager;

    private readonly EventManagerDbContext _context;

    public JwtHelper(IConfiguration configuration, EventManagerDbContext context)
    {
        _config = configuration;
        //_userManager = userManager;
        _context = context;
    }

    public async Task<string> GenerateJwtToken(string userId, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

        var user = await _context.Users.FindAsync(Guid.Parse(userId));
        var userEmail = user?.Email ?? string.Empty;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, userId),
            new Claim(ClaimTypes.Role, role),
            new Claim("firstName", user.FirstName ?? ""),
            new Claim("lastName", user.LastName ?? "")
        };

        if (!string.IsNullOrEmpty(userEmail))
        {
            claims.Add(new Claim(ClaimTypes.Email, userEmail));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}