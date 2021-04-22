using ArtistAwards.Data;
using DotNetAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks; 

namespace ArtistAwards.Services
{
  public class ArtistService
  {
    public ArtistService(IWebHostEnvironment webHostEnvironment, ArtistContext context)
    {
      WebHostEnvironment = webHostEnvironment;
      ArtistContext = context;
    }

    public IWebHostEnvironment WebHostEnvironment { get; }
    private ArtistContext ArtistContext;
    private IEnumerable<Artist> Artists;

    private string JsonFileName
    {
      get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "artists.json"); }
    }

    public IEnumerable<Artist> GetArtists()
    {
      Artists = ArtistContext.Artists;
      return Artists;
    }

    //public async Task<List<Artist>> GetArtists()
    //{
    //  Artists = await ArtistContext.Artists.ToListAsync();
    //  return Artists;
    //}

    public async Task VoteAsync(int id)
    {

      Artist artistToVote = await ArtistContext.Artists.FindAsync(id);

      //if (artistToVote == null)
      //{
      //  return NotFound();
      //}

      artistToVote.Votes += 1;


      await ArtistContext.SaveChangesAsync();
      

      //List<Artist> artists = await GetArtists();

      //Artist artist = artists.First(a => a.Id == id);
      //artist.Votes += 1;
      //artists.First(a => a.Id == id).Votes = artist.Votes;

      //using (var outputStream = File.OpenWrite(JsonFileName))
      //{
      //  JsonSerializer.Serialize<IEnumerable<Artist>>(
      //      new Utf8JsonWriter(outputStream, new JsonWriterOptions
      //      {
      //        SkipValidation = true,
      //        Indented = true
      //      }),
      //      artists
      //  );
      //}

    }
  }
}
