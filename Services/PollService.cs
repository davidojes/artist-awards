using DotNetAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ArtistAwards;
using ArtistAwards.Data;

namespace PollAwards.Services
{
  public class PollService
  {
    public PollService(IWebHostEnvironment webHostEnvironment, AppDbContext context)
    {
      WebHostEnvironment = webHostEnvironment;
      DbContext = context;
    }

    public IWebHostEnvironment WebHostEnvironment { get; }
    private AppDbContext DbContext;

    public async Task<Poll> GetPoll(Guid id)
    {
      Poll poll = await DbContext.Polls.
        Include(p => p.Status).
        Include(p => p.PollOptions).
        //Include(p => p.PollOptions.Select(po => po.Id)).
        //Include(p => p.PollOptions.Select(po => po.Content)).
        //Include(p => p.PollOptions.Select(po => po.Votes)).
        //Where().
        FirstOrDefaultAsync(p => p.Id == id);
      return poll;
    }

    public async Task<Poll> CreatePoll(Poll poll)
    {
      DbContext.Add(poll);
      await DbContext.SaveChangesAsync();

      return poll;
    }

    //public async Task VoteAsync(int id)
    //{

    //  Poll PollToVote = await DbContext.Polls.FindAsync(id);
    //  PollToVote.Votes += 1;

    //  await DbContext.SaveChangesAsync();
    //}

  }
}
