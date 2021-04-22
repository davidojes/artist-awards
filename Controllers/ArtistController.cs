using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArtistAwards.Services;

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

    [HttpPost]
    public async Task<Artist> CreateArtist([FromBody] Artist artist)
    {
      await ArtistService.CreateArtist(artist);

      return artist;
    }

    [Route("vote")]
    [HttpPost]
    public async Task VoteAsync([FromBody] Artist artist)
    {
      await ArtistService.VoteAsync(artist.Id);
      //return Ok();

    }
  }
}