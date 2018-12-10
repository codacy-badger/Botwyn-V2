using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using HyperEx;
using Microsoft.Extensions.DependencyInjection;
using Botwyn.Services;
using Victoria;
using Discord.Rest;
using Discord.Commands;
using Discord.WebSocket;

namespace Botwyn
{
    public class Bot
    {
        private readonly Assembly _assembly = Assembly.GetExecutingAssembly();

        private static Task Main(string[] args)
            => new Bot().InitializeAsync();

        private async Task InitializeAsync()
        {
            var provider = ConfigureServices();
            provider.InjectProperties(_assembly, typeof(InjectAttribute));
            await provider.GetRequiredService<DiscordService>().InitializeAsync(_assembly);
            await Task.Delay(Timeout.Infinite);
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection()
                .AddSingleton<Lavalink>()
                .AddSingleton<DiscordRestClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<DiscordSocketClient>()
                .RegisterSubclasses(_assembly, typeof(BaseService), true);

            return services.BuildServiceProvider();
        }
    }
}
