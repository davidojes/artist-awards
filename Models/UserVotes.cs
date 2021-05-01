using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ArtistAwards
{
    public partial class UserVotes
    {
        public int Id { get; set; }
        public Guid PollId { get; set; }
        public int UserId { get; set; }
        public int PollOptionId { get; set; }

        public virtual Poll Poll { get; set; }
        public virtual PollOption PollOption { get; set; }
    }
}
