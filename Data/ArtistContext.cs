using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetAPI;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ArtistAwards;

namespace ArtistAwards.Data
{
  public class ArtistContext : DbContext
  {
    public ArtistContext(DbContextOptions<ArtistContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<UserRole> Userroles { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public DbSet<Artist> Artists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Artist>().ToTable("Artists");
      modelBuilder.Entity<Artist>()
    .Property(e => e.Id)
    .ValueGeneratedOnAdd();
      //  modelBuilder.Entity<User>()
      //.Property(u => u.Id)
      //.ValueGeneratedOnAdd();
      modelBuilder.Entity<Role>(entity =>
      {
        entity.ToTable("roles");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(50);
      });

      modelBuilder.Entity<UserRole>(entity =>
      {
        entity.Property(e => e.Id).HasColumnName("id");

        entity.ToTable("userroles");

        entity.Property(e => e.Roleid).HasColumnName("roleid");

        entity.Property(e => e.Userid).HasColumnName("userid");

        entity.HasOne(d => d.Role)
            .WithMany()
            .HasForeignKey(d => d.Roleid)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("userroles_roleid_fkey");

        entity.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.Userid)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("userroles_userid_fkey");
      });

      modelBuilder.Entity<User>(entity =>
      {
        entity.ToTable("users");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.Email)
            .IsRequired()
            .HasColumnName("email")
            .HasMaxLength(255);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(50);

        entity.Property(e => e.Passwordhash)
            .IsRequired()
            .HasColumnName("passwordhash")
            .HasMaxLength(100);
      });

      //OnModelCreatingPartial(modelBuilder);
    }
    //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
  }





}
