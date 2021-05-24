using ArtistAwards.Data;
using ArtistAwards.Helper_Models;
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
    private Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

    [HttpPost, Route("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
      ValidateAuthModel(model);
      var response = UserService.AuthenticateUser(model.Email, model.Password);
      if (response == null)
        return BadRequest(new { message = "Email or Password is incorrect" });

      SetAuthTokens(response);

      return Ok();
    }

    [HttpPost, Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
      ValidateAuthModel(model);

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

    [HttpPost, Route("logout")]
    public IActionResult Logout()
    {
      var accessToken = Request.Cookies["accessToken"];
      var refreshToken = Request.Cookies["refreshToken"];
      var result = UserService.LogOut(accessToken, refreshToken);
      if (result == false) { return BadRequest(new { message = "We encountered an error while trying to log you out" }); }
      else { UnsetAuthTokens(); return Ok(); }
    }

    /*
     * Helper methods
     */

    public void SetAuthTokens(AuthResponse response)
    {

      var accessCookieOptions = new CookieOptions
      {
        HttpOnly = true,
        Expires = DateTime.UtcNow.AddDays(1)
      };

      var refreshCookieOptions = new CookieOptions
      {
        HttpOnly = true,
        Expires = DateTime.UtcNow.AddDays(30)
      };

      Response.Cookies.Append("accessToken", response.JwtToken, accessCookieOptions);
      Response.Cookies.Append("refreshToken", response.RefreshToken, refreshCookieOptions);
    }

    public void UnsetAuthTokens()
    {

      var cookieOptions = new CookieOptions
      {
        HttpOnly = true,
        Expires = DateTime.UtcNow.AddDays(1)
      };

      Response.Cookies.Append("accessToken", "invalid", cookieOptions);
      Response.Cookies.Append("refreshToken", "invalid", cookieOptions);
    }

    public BadRequestObjectResult ValidateAuthModel(AuthModel model)
    {
      Match emailMatch = EmailRegex.Match(model.Email);

      if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
      {
        return BadRequest(new { message = "Email and Password must be filled" });
      }
      if (!emailMatch.Success)
      {
        return BadRequest(new { message = "Invalid Email" });
      }

      return null;
    }
  }




  public interface AuthModel
  {
    public string Email { get; set; }
    public string Password { get; set; }
  }

  public class LoginModel : AuthModel
  {
    public string Email { get; set; }
    public string Password { get; set; }
  }

  public class RegisterModel : AuthModel
  {
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
  }

}
