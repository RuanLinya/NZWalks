using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NZWalks.API.Models.DTO;
using Microsoft.AspNetCore.Identity;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }
        //Post api/auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
           var identityUser = new IdentityUser
           {
               UserName = registerRequestDto.Username,
               Email = registerRequestDto.Username
           };

           var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
              if(identityResult.Succeeded)
              {
                //Assign Roles to User
                if(registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                     identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                        if(!identityResult.Succeeded)
                        {

                             return Ok("User was registered! Please Login");
                        }
                return Ok();
                }
             
              }
                return BadRequest("Something went wrong");
        }

        //Post api/auth/Login               
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user= await userManager.FindByNameAsync(loginRequestDto.Username);
            if(user != null)
            {
                var checkPassword = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if(checkPassword)
                {
                    // Get roles for the user
                    var roles = await userManager.GetRolesAsync(user);

                    if(roles != null && roles.Any())
                    {
                        // Create Token with Roles
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };

                        return Ok(response);
                    }
                    // Create Token

                    return Ok();
                }
            }
            return BadRequest("Invalid username or password");
        }
    }
}

