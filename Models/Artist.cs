using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DotNetAPI
{
  public class Artist
  {
    // JsonPropertyName maps an object field to JSON names
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("votes")]
    public long Votes { get; set; }

    public Artist()
    {
    }

    public Artist(int id, string name, long votes)
    {
      Id = id;
      Name = name;
      Votes = votes;
    }

    public override string ToString() => JsonSerializer.Serialize<Artist>(this);

    // this method converts the object to json based on the names mapped

    }
  }
