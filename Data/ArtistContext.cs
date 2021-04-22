using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetAPI;

namespace ArtistAwards.Data
{
  public class ArtistContext : DbContext
  {
    public ArtistContext(DbContextOptions<ArtistContext> options)
        : base(options)
    {
    }

    public DbSet<Artist> Artists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Artist>().ToTable("Artists");
      modelBuilder.Entity<Artist>()
    .Property(e => e.Id)
    .ValueGeneratedOnAdd();
    }
  }
}
