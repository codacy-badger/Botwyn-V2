using System.Collections.Generic;
using Discord;
using Victoria.Entities;

namespace Botwyn.Objects
{
    public struct AudioOptions
    {
        public bool Shuffle { get; set; }
        public bool RepeatTrack { get; set; }
        public IUser Summoner { get; set; }
        public LavaTrack VotedTrack { get; set; }
        public HashSet<ulong> Voters { get; set; }
    }
}
