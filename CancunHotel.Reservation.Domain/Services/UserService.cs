using CancunHotel.Reservation.Domain.Exceptions;
using CancunHotel.Reservation.Domain.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CancunHotel.Reservation.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IClock clock;

        public UserService(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager,
            IHttpContextAccessor contextAccessor,
            IClock clock)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.contextAccessor = contextAccessor;
            this.clock = clock;
        }

        public async Task<string> Login(string email, string password)
        {
            var result = await signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new ValidationException("Login failed.", null);
            }

            return await BuildToken(email);
        }

        public async Task<string> Register(string email, string password)
        {
            var result = await userManager.CreateAsync(new IdentityUser
            {
                Email = email,
                UserName = email
            }, password);

            if (!result.Succeeded)
            {
                var errorMessage = $"One or more errors ocurred during the registration process: {string.Join(", ", result.Errors.Select(_ => _.Description))}";
                throw new ValidationException(errorMessage, null);
            }

            return await BuildToken(email);
        }

        public string GetLoggedInUserId()
        {
            return userManager.GetUserId(contextAccessor.HttpContext.User);
        }

        private async Task<string> BuildToken(string email)
        {
            var user = await userManager.FindByNameAsync(email);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwTKey"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, user.Id), new Claim(ClaimTypes.Name, email) }),
                Expires = clock.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
