using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Botwyn.Objects
{
    public class Affixes
    {
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("leaderboard_url")]
        public string LeaderboardUrl { get; set; }
        [JsonProperty("affix_details")]
        public Affix_Details[] AffixDetails { get; set; }
    }

    public class Affix_Details
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("wowhead_url")]
        public string WowheadUrl { get; set; }
    }

}

