﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Domain.Abstractions;
using DiscordBot.Domain.Configuration;
using DiscordBot.Modules.Services;
using DiscordBot.Services.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LibMCRcon.RCon;

namespace DiscordBot
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddOptions()
                        .AddHttpClient()

                        .AddScoped<IWeatherService, WeatherService>()
                        .AddScoped<ICatService, CatService>()

                        .AddSingleton<DiscordSocketClient>()
                        .AddSingleton<CommandService>()
                        .AddSingleton<CommandHandlingService>()
                        .AddSingleton<DiscordLoggingService>()
                        ;
                    services.Configure<DiscordSettings>(hostContext.Configuration.GetSection("Discord"));
                    services.Configure<ImgurSettings>(hostContext.Configuration.GetSection("Imgur"));

                    services.AddHostedService<BotService>();
                });
    }
}