using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;

        }

        [HttpPost("register")]

        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // validate request

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower(); // change username to lower case

            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already taken");

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);

        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(
                userForLoginDto.Username.ToLower(), // we always login with lower case username
                userForLoginDto.Password);

            // First we check if we have user 
            // with username and password match what is stored in our database
            // if not we return unauthorised
            if (userFromRepo == null)
                return Unauthorized();

            // If Yes, there is a match in our database then
            // we start building our token
            // our token contains 2 claims: Id and username
            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    userFromRepo.Id.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    userForLoginDto.Username)
            };

            // In order to make sure that the tokens are valid tokens when it comes back
            // The server needs to signs this token by using security key
            // and use this security key as a part of Signing Credential
            // The security key is located at appsettings.dev.json w
            // which needs to keep secret from anyone
            // the token needs to be at least 12 characters long

            var key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(_config.GetSection("AppSetting:Token").Value));

            // This SigningCredentials method takes our security key that we just generated above
            // and the algorithm we are going to use to hash this security key

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha512Signature);

            // Now we need to create a security token descriptor which contains
            // our claims , expiry date for our token and also the signing credentials
            // which is used to create a security token.

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            // We also need to have a token handler 

            var tokenHandler = new JwtSecurityTokenHandler();

            // And now using token handler we can create a token and pass in the token descriptor

            var token = tokenHandler.CreateToken(tokenDescriptor);

            // and this token is going to contain our jwt token that we want to return to client

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}