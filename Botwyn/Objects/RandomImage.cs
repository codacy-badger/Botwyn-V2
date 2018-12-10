using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Botwyn.Objects
{
    public class RandomDog
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("message")]
        public string ImageUrl { get; set; }
    }

    public class RandomCat
    {
        [JsonProperty("file")]
        public string ImageUrl { get; set; }
    }
    public class RandomMeme
    {
        [JsonProperty("url")]
        public string ImageUrl { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
