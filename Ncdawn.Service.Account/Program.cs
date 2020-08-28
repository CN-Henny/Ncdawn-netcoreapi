using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Ncdawn.Core.Config;

namespace Ncdawn.EFS.Service.Account
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //实例化一个配置生成器         
            var config = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          //.AddJsonFile("appsettings.json", optional: true)//添加配置文件hosting.json 
          .AddConfigFile("${Ncdawn_appsettings}|appsettings.json", optional: true)
          .Build();
            Core.DotNetty.Program.RunServerAsync(config);
            var port = config.GetSection("eureka").GetSection("instance")["port"];
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:" + port)
                .UseConfiguration(config)//使用配置信息         
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();
            host.Run();
        }
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

    }
}
