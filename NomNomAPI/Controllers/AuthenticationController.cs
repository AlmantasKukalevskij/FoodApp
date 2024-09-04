namespace NomNomAPI.Controllers;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NomNomAPI.Configurations;
using NomNomAPI.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


//specifying route
[Route("api/[controller]")] //api/authentication
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    //private readonly JwtConfig _jwtConfig;

    public AuthenticationController(UserManager<IdentityUser> userManager, IConfiguration configuration /*JwtConfig jwtConfig*/)
    {
        _userManager = userManager;
        //_jwtConfig = jwtConfig;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto requestDto)
    {
        // validuojam ateinancia uzklausa
        if (ModelState.IsValid)
        {
            //tikrinam ar pastas jau egzistuoja
            var user_exist = await _userManager.FindByEmailAsync(requestDto.Email);
            if (user_exist != null)
            {
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Pastas jau uzimtas"
                    }

                });
            }
            //sukuriam vartotoja
            var new_user = new IdentityUser()
            {
                Email = requestDto.Email,
                UserName = requestDto.Email
            };
            var is_created = await _userManager.CreateAsync(new_user, requestDto.Password);

            if (is_created.Succeeded)
            {
                //Sukuriam tokena
                var token = GenerateJwtToken(new_user);
                return Ok(new AuthResult()
                    {
                        Result = true,
                        Token = token
                    });
            }
            return BadRequest(new AuthResult()
            {
                Errors = is_created.Errors.Select(e => e.Description).ToList(),
                Result = false
            });

        }
        return BadRequest(ModelState);

    }

    [Route("Login")]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDto loginRequest)
    {
        if(ModelState.IsValid) 
        {
            // Tikrinam ar useris egzistuoja
            var existing_user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if(existing_user == null)
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Error"
                    },
                    Result = false
                });
            var isCorrect = await _userManager.CheckPasswordAsync(existing_user, loginRequest.Password);

            if (!isCorrect)
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Error"
                    },
                    Result = false
                });

            // jeigu geras, generuojam JWT tokena
            var jwtToken = GenerateJwtToken(existing_user);

            return Ok(new AuthResult()
            {
                Token = jwtToken,
                Result = true
            });
                    
        }
        return BadRequest(new AuthResult()
        {
            Errors = new List<string>()
            {
                "Error"
            },
            Result = false
        });
    }
    private string GenerateJwtToken(IdentityUser user)
    {
        //token handler, generuoja tokena
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);//duos rakto bitu array

        //Token descriptor, informacija apie tokena (header, payload, signature)
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new []// list of claims, is ko sudarytas JWT
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.Now.ToUniversalTime().ToString())
            }),
            //nusakom kiek laiko galios tokenas
            Expires = DateTime.Now.AddHours(1),
            //security to authenticate all the requests
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        return jwtTokenHandler.WriteToken(token); // konvertuojam tokena i teksta(string)

    }

}

