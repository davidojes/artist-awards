using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ArtistAwards.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    [HttpPost, Route("login")]
    public IActionResult Login([FromBody] LoginModel user)
    {
      if (user.UserName == "david" && user.Password == "pass")
      {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretkey1234567"));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokeOptions = new JwtSecurityToken(
            issuer: "http://localhost:5000",
            audience: "http://localhost:5000",
            claims: new List<Claim>(),
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: signinCredentials
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        return Ok(new { Token = tokenString });
      }
      else
      {
        return Unauthorized();
      }
    }
  }

  public class LoginModel
  {
    public string UserName { get; set; }
    public string Password { get; set; }
  }
}
