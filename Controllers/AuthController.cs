using ArtistAwards.Data;
using ArtistAwards.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace ArtistAwards.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    public AuthController(IConfiguration configuration, UserService userService, AppDbContext dbContext)
    {
      Config = configuration;
      UserService = userService;
      DbContext = dbContext;
    }

    private IConfiguration Config { get; }
    private UserService UserService;
    private AppDbContext DbContext;

    [HttpPost, Route("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
      Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
      Match emailMatch = regex.Match(model.Email);

      if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
      {
        return BadRequest(new { message = "Email and Password must be filled" });
      }
      if (!emailMatch.Success)
      {
        return BadRequest(new { message = "Invalid Email" });
      }

      var user = UserService.AuthenticateUser(model.Email, model.Password);
      if (user == null)
        return BadRequest(new { message = "Email or Password is incorrect" });

      var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.GetValue<string>("SecretKey")));
      var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
      var roles = UserService.GetUserRoles(user.Id);
      var claims = new List<Claim>();
      foreach (Role role in roles)
      {
        var claim = new Claim(ClaimTypes.Role, role.Name);
        claims.Add(claim);
      }
      var tokeOptions = new JwtSecurityToken(
          issuer: "http://localhost:5000",
          audience: "http://localhost:5000",
          claims: claims,
          expires: DateTime.Now.AddMinutes(120),
          signingCredentials: signinCredentials
      );
      var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
      return Ok(new { Token = tokenString });


    }

    [HttpPost, Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
      Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
      Match emailMatch = regex.Match(model.Email);

      if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.Name))
      {
        return BadRequest(new { message = "Name, Email, and Password must be filled" });
      }
      if (!emailMatch.Success)
      {
        return BadRequest(new { message = "Invalid Email" });
      }

      var user = new User { Name = model.Name, Email = model.Email };
      string passwordHash = BC.HashPassword(model.Password);
      user.Passwordhash = passwordHash;
      User checkUser = DbContext.Users.SingleOrDefault(u => u.Email == user.Email);
      if (checkUser != null)
      {
        return BadRequest(new { message = "User already exists" });
      }

      await UserService.CreateUser(user);

      return Ok();
    }
  }


  public class LoginModel
  {
    public string Email { get; set; }
    public string Password { get; set; }
  }

  public class RegisterModel
  {
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
  }

}
