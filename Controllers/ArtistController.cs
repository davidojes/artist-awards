using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArtistAwards.Services;
using Microsoft.AspNetCore.Authorization;

namespace DotNetAPI
{
  [Route("api/[controller]")]
  [ApiController]
  public class ArtistController : ControllerBase
  {
    public ArtistController(ArtistService _artistService)
    {
      ArtistService = _artistService;
    }

    public ArtistService ArtistService;

    [HttpGet]
    public IEnumerable<Artist> GetArtists()
    {
      return ArtistService.GetArtists();
      //string artistsJson = JsonSerializer.Serialize(ArtistService.GetArtists());
      //return artistsJson;
    }

    [RouteAttribute("{id}")]
    [HttpGet]
    public async Task<Artist> GetArtist(int id)
    {
      return await ArtistService.GetArtist(id);
      //string artistsJson = JsonSerializer.Serialize(ArtistService.GetArtists());
      //return artistsJson;
    }

    [HttpPost, Authorize(Roles = "manager")]
    public async Task<Artist> CreateArtist([FromBody] Artist artist)
    {
      await ArtistService.CreateArtist(artist);

      return artist;
    }

    [Route("vote")]
    [HttpPost, Authorize(Roles = "voter")]
    public async Task<StatusCodeResult> VoteAsync([FromBody] Artist artist)
    {
      await ArtistService.VoteAsync(artist.Id);
      return Ok();

    }
  }
}