using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using JwtDemo.WebDemo.EF.Models;
using JwtDemo.WebDemo.Helpers;
using JwtDemo.WebDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtDemo.WebDemo.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtHandler _jwtHandler;

        public AuthenticationController(UserManager<ApplicationUser> userManager, JwtHandler jwtHandler)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userForAuthentication)
        {
            var user = await _userManager.FindByNameAsync(userForAuthentication.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
            {
                return Unauthorized(new UserLoginResponse { ErrorMessage = "Invalid Authentication" });
            }

            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = await _jwtHandler.GetClaimsAsync(user);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new UserLoginResponse { IsAuthSuccessful = true, Token = token });
        }
    }
}
