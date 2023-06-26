using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HotelBooking.Data;
using HotelBooking.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Azure;

namespace HotelBooking.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly HotelBookingDbContext context;
        public AuthenticationController(HotelBookingDbContext hotelBookingDbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            context = hotelBookingDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        [Route("api/[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserRegistration userRegistration, string role)
        {

            var userexist = await _userManager.FindByEmailAsync(userRegistration.Email);
            if (userexist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Responses { Status = "Error", Message = "user Already exist" });
            }
            IdentityUser user = new()
            {
                Email = userRegistration.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = userRegistration.Name,
                PhoneNumber = userRegistration.Phonenumber.ToString()
            };
            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user, userRegistration.Password);
                //return result.Succeeded ? StatusCode(StatusCodes.Status201Created,
                // new Response { Status = "Success", Message = "User created successfuly" }) :
                //StatusCode(StatusCodes.Status500InternalServerError,
                //new Response { Status = "Success", Message = "User Faild to created" });
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                           new Responses { Status = "Success", Message = "User Faild to created" });
                }
                await _userManager.AddToRoleAsync(user, role);
                return StatusCode(StatusCodes.Status201Created,
                 new Responses { Status = "Success", Message = "User created successfuly" });
            }

            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Responses { Status = "Success", Message = "User Faild to created" });
            }
            //context.UserRegistrations.Add(userRegistration);
            //context.SaveChanges();
            //return Ok("User Added successfully");
        }
        [Route("api/[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
                var authClaim = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };
                var userroles = await _userManager.GetRolesAsync(user);
                foreach (var role in userroles)
                {
                    authClaim.Add(new Claim(ClaimTypes.Role, role));
                }
                var jwttoken = GetToken(authClaim);
                var tokenHandler = new JwtSecurityTokenHandler();
                return Ok(new
                {
                    token = tokenHandler.WriteToken(jwttoken),
                    expiration = jwttoken.ValidTo
                });

            }
            return Unauthorized();
        }
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:ValidIssuer"],
                audience: _configuration["Jwt:ValidAudience"],
                expires: DateTime.Now.AddHours(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

    }
}
