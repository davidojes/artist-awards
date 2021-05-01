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
using BC = BCrypt.Net.BCrypt;

namespace ArtistAwards.Services
{
  public class UserService
  {
    public UserService( AppDbContext context)
    {
      ArtistContext = context;
    }

    private AppDbContext ArtistContext;
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

    public async Task<User> CreateUser(User user)
    {     
      ArtistContext.Users.Add(user);
      await ArtistContext.SaveChangesAsync();
      User registeredUser = ArtistContext.Users.SingleOrDefault(u => u.Email == user.Email);
      UserRole ur = new UserRole(1, registeredUser.Id);
      ArtistContext.Userroles.Add(ur);
      await ArtistContext.SaveChangesAsync();

      return user;
    }

    public User AuthenticateUser(string email, string password)
    {
      var user =  ArtistContext.Users.SingleOrDefault(u => u.Email == email);

      if(user == null)
      {
        return null;
      }

      if (!BC.Verify(password, user.Passwordhash))
        return null;

      return user;
    }


    public IEnumerable<Role> GetUserRoles(int userId)
    {
      var userRoleIds =  ArtistContext.Userroles.Where(ur => ur.Userid == userId).Select(ur => ur.Roleid).ToList();
      var userRoles = ArtistContext.Roles.Where(r => userRoleIds.Contains(r.Id)).ToList();
      return userRoles;
    }




    //public async Task VoteAsync(int id)
    //{

    //  User UserToVote = await ArtistContext.Users.FindAsync(id);
    //  UserToVote.Votes += 1;

    //  await ArtistContext.SaveChangesAsync();
    //}

  }
}
