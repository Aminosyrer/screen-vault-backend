using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieDatabase.Models;
using MovieDatabase.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;

namespace MovieDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            var user = _userService.GetByUsername(loginModel.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, user.Password))
            {
                return Unauthorized("Invalid username or password");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel registerModel)
        {
            var existingUser = _userService.GetByUsername(registerModel.Username);

            if (existingUser != null)
            {
                return BadRequest("User already exists");
            }

            var user = new User
            {
                Username = registerModel.Username,
                Email = registerModel.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerModel.Password),
                Roles = new System.Collections.Generic.List<string> { "user" }
            };

            _ = _userService.Create(user);

            return Ok(new { Message = "User registered successfully!" });
        }

        private string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["JWT_KEY"];
            var jwtIssuer = _configuration["JWT_ISSUER"];
            var jwtAudience = _configuration["JWT_AUDIENCE"];

            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                throw new ArgumentNullException("JWT settings are not configured properly.");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Roles.FirstOrDefault() ?? string.Empty)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}