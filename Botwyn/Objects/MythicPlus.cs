using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Botwyn.Objects
{
    public class MythicPlus
    {
        public string name { get; set; }
        public string race { get; set; }
        [JsonProperty("class")]
        public string _class { get; set; }
        public string active_spec_name { get; set; }
        public string active_spec_role { get; set; }
        public string gender { get; set; }
        public string faction { get; set; }
        public int achievement_points { get; set; }
        public int honorable_kills { get; set; }
        public string thumbnail_url { get; set; }
        public string region { get; set; }
        public string realm { get; set; }
        public string profile_url { get; set; }
        public Mythic_Plus_Scores mythic_plus_scores { get; set; }
        public Gear gear { get; set; }
        public Raid_Progression raid_progression { get; set; }
        public Guild guild { get; set; }
    }

    public class Mythic_Plus_Scores
    {
        public float all { get; set; }
        public float dps { get; set; }
        public float healer { get; set; }
        public float tank { get; set; }
    }

    public class Gear
    {
        public float item_level_equipped { get; set; }
        public float item_level_total { get; set; }
        public float artifact_traits { get; set; }
    }

    public class Raid_Progression
    {
        public AntorusTheBurningThrone antorustheburningthrone { get; set; }
        public TheEmeraldNightmare theemeraldnightmare { get; set; }
        public TheNighthold thenighthold { get; set; }
        public TombOfSargeras tombofsargeras { get; set; }
        public TrialOfValor trialofvalor { get; set; }
        public Uldir uldir { get; set; }
    }

    public class AntorusTheBurningThrone
    {
        public string summary { get; set; }
        public int total_bosses { get; set; }
        public int normal_bosses_killed { get; set; }
        public int heroic_bosses_killed { get; set; }
        public int mythic_bosses_killed { get; set; }
    }

    public class TheEmeraldNightmare
    {
        public string summary { get; set; }
        public int total_bosses { get; set; }
        public int normal_bosses_killed { get; set; }
        public int heroic_bosses_killed { get; set; }
        public int mythic_bosses_killed { get; set; }
    }

    public class TheNighthold
    {
        public string summary { get; set; }
        public int total_bosses { get; set; }
        public int normal_bosses_killed { get; set; }
        public int heroic_bosses_killed { get; set; }
        public int mythic_bosses_killed { get; set; }
    }

    public class TombOfSargeras
    {
        public string summary { get; set; }
        public int total_bosses { get; set; }
        public int normal_bosses_killed { get; set; }
        public int heroic_bosses_killed { get; set; }
        public int mythic_bosses_killed { get; set; }
    }

    public class TrialOfValor
    {
        public string summary { get; set; }
        public int total_bosses { get; set; }
        public int normal_bosses_killed { get; set; }
        public int heroic_bosses_killed { get; set; }
        public int mythic_bosses_killed { get; set; }
    }

    public class Uldir
    {
        public string summary { get; set; }
        public int total_bosses { get; set; }
        public int normal_bosses_killed { get; set; }
        public int heroic_bosses_killed { get; set; }
        public int mythic_bosses_killed { get; set; }
    }

    public class Guild
    {
        public string name { get; set; }
        public string realm { get; set; }
    }

}


