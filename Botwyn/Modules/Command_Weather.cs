using Botwyn.Handlers;
using Botwyn.Modules.Custom;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Botwyn.Modules
{
    public class Command_Weather : CustomModule
    {
        [Command("weather"), Name("Weather"), Summary("Gets the current weather status for your search.")]
        public async Task Weather([Remainder]string search)
        {
            await ReplyAsync($"This is a test result: {WeatherHandler.GetWeather(search)}");
        }
    }
}
