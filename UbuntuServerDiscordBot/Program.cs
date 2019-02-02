using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UbuntuServerDiscordBot
{
    class Program
    {

        public static IConfiguration Configuration { get; private set; }
        public static ILogger Logger { get; private set; }

        static void Main(string[] args)
        {
            if (args.Contains("--help"))
            {
                Console.WriteLine("No help configured at this time");
                Environment.Exit(404);
            }

            var startTimeString = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ssZ");
            if (!Directory.Exists("logs"))
            {
                Directory.CreateDirectory("logs");
            }
            Logger = new LoggerFactory().CreateLogger($@"logs\{startTimeString}.log");
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("botconfig.json")
                .Build();

            var bot = new Bot(Configuration, Logger);
            var botStartupTask = bot.StartAsync();
            botStartupTask.Wait(); // this is the waiting for messages state
        }
    }
}
