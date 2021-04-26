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
  public class UserService
  {
    public UserService( ArtistContext context)
    {
      ArtistContext = context;
    }

    private ArtistContext ArtistContext;
    private IEnumerable<User> Users;

    public IEnumerable<User> GetUsers()
    {
      Users = ArtistContext.Users;
      return Users;
    }

    public async Task<User> GetUser(int id)
    {
      User User = await ArtistContext.Users.FindAsync(id);
      return User;
    }

    public async Task<User> CreateUser(User User)
    {
      ArtistContext.Add(User);
      await ArtistContext.SaveChangesAsync();

      return User;
    }

    //public async Task VoteAsync(int id)
    //{

    //  User UserToVote = await ArtistContext.Users.FindAsync(id);
    //  UserToVote.Votes += 1;

    //  await ArtistContext.SaveChangesAsync();
    //}

  }
}
