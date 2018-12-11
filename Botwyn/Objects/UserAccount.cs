using System;
using System.Collections.Generic;
using System.Text;
using Botwyn.Objects;

namespace Botwyn.Objects
{
    public class UserAccount
    {
        public ulong UserID { get; set; }
        public string MainChar { get; set; }
        public string MainSpec { get; set; }
        public string WowAlt { get; set; }
        public string WowAltSpec { get; set; }
        public string GuildRank { get; set; }
        public int ReportMade { get; set; }
        public int OwnReports { get; set; }
        public int AdminReports { get; set; }
        public int RaidsMissedWithReason { get; set; }
        public int RaidsMissedNoReason { get; set; }
        public bool ReturningForNextRaid { get; set; }
        public bool IsTrial { get; set; }
    }
}
