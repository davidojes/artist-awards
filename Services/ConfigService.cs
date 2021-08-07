using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtistAwards.Services
{
  public class ConfigService
  {
    public CookieOptions AccessCookieOptions { get; set; }
    public CookieOptions RefreshCookieOptions { get; set; }
    public IWebHostEnvironment Env { get; set; }

    public ConfigService(IWebHostEnvironment env)
    {
      Env = env;

      if (Env.IsDevelopment())
      {
        AccessCookieOptions = new CookieOptions
        {
          HttpOnly = false,
          Expires = DateTime.UtcNow.AddDays(1),
          SameSite = SameSiteMode.None,
          Secure = false
        };

        RefreshCookieOptions = new CookieOptions
        {
          HttpOnly = true,
          Expires = DateTime.UtcNow.AddDays(30),
          SameSite = SameSiteMode.None,
          Secure = false
        };
      }
      else
      {
        AccessCookieOptions = new CookieOptions
        {
          HttpOnly = false,
          Expires = DateTime.UtcNow.AddDays(1),
          SameSite = SameSiteMode.None,
          Secure = true,
          Domain = "davidojes.dev"
        };

        RefreshCookieOptions = new CookieOptions
        {
          HttpOnly = true,
          Expires = DateTime.UtcNow.AddDays(30),
          SameSite = SameSiteMode.None,
          Secure = true,
          Domain = "davidojes.dev"
        };
      }

    }

    public CookieOptions GetAccessCookieOptions()
    {
      return AccessCookieOptions;
    }

    public CookieOptions GetRefreshCookieOptions()
    {
      return RefreshCookieOptions;
    }
  }
}
