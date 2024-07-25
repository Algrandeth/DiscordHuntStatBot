using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using static GrandDiscordBot.Structures;

namespace GrandDiscordBot
{
    public class Program
    {
        private static readonly DiscordSocketClient client = new();
        private static readonly HttpClient http = new()
        {
            Timeout = TimeSpan.FromMinutes(3)
        };

        private static readonly ulong voiceChannelID = Convert.ToUInt64(Environment.GetEnvironmentVariable("id"));

        static async Task Main() => await Init();


        private async static Task Init()
        {
            client.Log += Log;

            await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("token"));

            await LogMessage("Started");
            await client.StartAsync();
            await RunBackgroundShit();
        }

        private static async Task Log(LogMessage msg) => await LogMessage(msg.Message);


        private static async Task RunBackgroundShit()
        {
            while (client.ConnectionState != ConnectionState.Connected)
                await Task.Delay(1000);

            if (!client.Guilds.Any())
            {
                await LogMessage("Bot have no connected guilds. BackgroundWorker stopped.");
                return;
            }

            var myGuild = client.Guilds.First(a => a.Id == 1262738263538532414);

            while (true)
            {
                try
                {
                    var steamApiRequest = await http.GetAsync("https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/?appid=594650");
                    var steamApiRequestResult = await steamApiRequest.Content.ReadAsStringAsync();

                    var huntPlayerCount = JsonConvert.DeserializeObject<Structures.Root>(steamApiRequestResult)!.Response.PlayerCount;

                    var statVoiceChannel = myGuild.GetVoiceChannel(voiceChannelID);
                    await statVoiceChannel.ModifyAsync(properties =>
                    {
                        properties.Name = $"ONLINE: {huntPlayerCount}";
                    });

                    await LogMessage($"Hunt player count: {huntPlayerCount}. Voicechannel name should be updated.");
                }
                catch (Exception ex)
                {
                    await LogMessage(ex.Message);
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromMinutes(10));
                }
            }
        }

        private async static Task LogMessage(string msg)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                var logText = $"| {DateTime.UtcNow.AddHours(3):HH:mm:ss yyyy-MM-dd} |   INFO   |  " + msg;

                Console.WriteLine(logText);
                Console.ResetColor();
            }
            catch
            { }
        }
    }
}
