using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArtistAwards.Models
{
  public class User
  {
    [Key]
    public int Id { get; set; }
    public string DisplayName { get; set; }
  }
}
