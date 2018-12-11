using Botwyn.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Botwyn.Handlers
{
    public static class GlobalProperties
    {
        public static string ConfigPath { get; set; } = "config.json";
        public static BotConfig Config { get; set; }
        public static ulong ReactionMessageID { get; set; }

        public static void Initialize()
        {
            var json = string.Empty;
            if (!File.Exists(ConfigPath))
            {
                json = JsonConvert.SerializeObject(Config);
                File.WriteAllText("config.json", json, new UTF8Encoding(false));
                Console.WriteLine("Config file was not found, a new one was generated. Fill it with proper values and rerun this program");
                Console.ReadKey();

                return;
            }
            json = File.ReadAllText(ConfigPath, new UTF8Encoding(false));
            Config = JsonConvert.DeserializeObject<BotConfig>(json);
        }
    }
}
