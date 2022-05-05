using AsendiaParcelScraper.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.Extensions.Logging;

namespace AsendiaParcelScraper
{
    public class Program
    {
        static JObject o1 = JObject.Parse(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"JSONs\Messages.json"));

        public static Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();

            Console.WriteLine(LoadDataUsingDI(host.Services));

            return host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddLogging(configure => configure.AddConsole())
                            .AddTransient<IScanDataRepository, ScanDataRepository>());
        }

        public static string LoadDataUsingDI(IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            var scanDataService = provider.GetRequiredService<IScanDataRepository>();

            string dirPath = string.Empty;
            bool scanComplete = true;
            try
            {
                Console.WriteLine((string)o1["UtilityHeader"]);

                do
                {
                    Console.WriteLine((string)o1["EnterPath"]);

                    dirPath = @"" + Console.ReadLine();
                    if (!Directory.Exists(dirPath))
                    {
                        Console.WriteLine((string)o1["NoValidPath"]);
                    }
                    else
                    {
                        scanComplete = scanDataService.LoadCsvData(dirPath);
                    }
                } while (scanComplete);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return scanComplete ? (string)o1["Failed"] : (string)o1["Success"];
        }
    }
}
