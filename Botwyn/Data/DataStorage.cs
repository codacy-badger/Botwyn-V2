using System.Collections.Generic;
using System.IO;
using Botwyn.Handlers;
using Botwyn.Objects;
using Botwyn.Services;
using Newtonsoft.Json;

namespace Botwyn.Data
{
    public static class DataStorage
    {
        //Save all userAccounts
        public static void SaveUserAccounts(IEnumerable<UserAccount> accounts, string filePath)
        {
            string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
            //using (TextWriter tw = new StreamWriter(filePath))
            //{
            //    tw.WriteLine(json);
            //}
            File.WriteAllText(filePath, json);
        }

        public static void SaveConfig(string filepath)
        {
            if (SaveExists(filepath))
            {
                string json = JsonConvert.SerializeObject(GlobalProperties.Config, Formatting.Indented);
                File.WriteAllText(filepath, json);
            }
            else
            {
                LoggingService.LogCritical("Discord", "Config File Does Not Exists.");
            }
        }
        //Get all userAccounts
        public static IEnumerable<UserAccount> GetUserAccounts(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<UserAccount>>(json);
        }

        public static bool SaveExists(string filePath)
        {
            return File.Exists(filePath);
        }

    }
}
