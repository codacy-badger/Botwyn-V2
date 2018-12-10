using Botwyn.Modules.Custom;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Botwyn.Modules
{
    public class Misc : CustomModule
    {
        [Command("roll"), Summary("Generates a random numbered roll between 0 - 100."), Name("Roll")]
        public async Task Roll(int min = 0, int max = 100)
        {
            if (min < 0 || max > 10000) { await ReplyAsync("Min/Max set to (0/10000)"); return; }

            await Context.Message.DeleteAsync();
            var rnd = new Random();
            await ReplyAsync($"{Context.User.Username} rolled: {rnd.Next(min, max + 1)} ({min}/{max}).");
        }

        [Command("Korwyn"), Summary("Detailed Video about Korwyn."), Name("Korwyn")]
        public async Task Korwyn()
        {
            await ReplyAsync("https://www.youtube.com/watch?v=bphPp8k9aZo");
        }
    }
}
