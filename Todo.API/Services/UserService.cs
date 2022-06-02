using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Todo.API.Commands;
using Todo.API.Entities;
using Todo.API.Exceptions;
using Todo.API.Infrastructure;
using Todo.API.Settings;

namespace Todo.API.Services
{
    public class UserService : IUserService
    {
        private readonly TodoDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JwtSettings _jwtSettings;

        public UserService(TodoDbContext context, IPasswordHasher<User> passwordHasher,
            IOptionsSnapshot<JwtSettings> authenticationSettings)
        {
            _dbContext = context;
            _passwordHasher = passwordHasher;
            _jwtSettings = authenticationSettings.Value;
        }

        public async Task<Guid> Register(RegisterUser command)
        {
            var newUser = new User(Guid.NewGuid(), command.Email);

            var hashedPassword = _passwordHasher.HashPassword(newUser, command.Password);

            newUser.SetPasswordHash(hashedPassword);

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return newUser.Id;
        }

        public async Task<string> Login(LoginUser command)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == command.Email) ??
                throw new BadRequestException("Invalid email or password");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, command.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_jwtSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_jwtSettings.JwtIssuer,
                _jwtSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}