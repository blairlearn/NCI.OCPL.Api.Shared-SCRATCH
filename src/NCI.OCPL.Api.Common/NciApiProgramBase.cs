using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace NCI.OCPL.Api.Common
{
  public abstract class NciApiProgramBase
  {
    /// <summary>
    /// CreateWebHostBuilder
    /// </summary>
    public static IWebHostBuilder CreateWebHostBuilder<TStartup>(string[] args)
      where TStartup : NciStartupBase =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<TStartup>()
            .ConfigureLogging((hostingContext, logging) =>
            {
              logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
              logging.AddConsole();
              logging.AddDebug();
            });

  }
}