using System;
using System.IO;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Nest;
using Xunit;

using NCI.OCPL.Api.Common.Controllers;

namespace NCI.OCPL.Api.Common
{
  /// <summary>
  /// Tests for NciStartupBase.ConfigureServices
  /// </summary>
  public partial class NciStartupBaseTest
  {
    /// <summary>
    /// Verify that subclasses of NciStartupBase have a chance to add required services.
    /// </summary>
    [Fact]
    public void ConfigureServices_SubclassSetupServices()
    {
      IHostingEnvironment hostenv = new MockHostingEnvironment();

      Mock<NciStartupBase> mockStartup = new Mock<NciStartupBase>(hostenv){ CallBase = true };

      mockStartup.Protected().Setup("AddAdditionalConfigurationMappings", Moq.Protected.ItExpr.IsAny<IServiceCollection>());
      mockStartup.Protected().Setup("AddAppServices", Moq.Protected.ItExpr.IsAny<IServiceCollection>());

      NciStartupBase startup = mockStartup.Object;

      IServiceCollection svcColl = new ServiceCollection();
      IServiceProvider serviceProvider = startup.ConfigureServices(svcColl);

      // Verify the subclass had a chance to add services.
      mockStartup.Protected().Verify("AddAdditionalConfigurationMappings", Times.Once(), Moq.Protected.ItExpr.IsAny<IServiceCollection>());
      mockStartup.Protected().Verify("AddAppServices", Times.Once(), Moq.Protected.ItExpr.IsAny<IServiceCollection>());
    }

    /// <summary>
    /// Fail to get an Elasticsearch client instance when no servers are configured.
    /// </summary>
    [Fact]
    public void ConfigureServices_ElasticsearchBadConfiguration()
    {
      IHostingEnvironment hostenv = new MockHostingEnvironment();

      Mock<NciStartupBase> mockStartup = new Mock<NciStartupBase>(hostenv) { CallBase = true };

      NciStartupBase startup = mockStartup.Object;

      IServiceCollection svcColl = new ServiceCollection();
      IServiceProvider serviceProvider = startup.ConfigureServices(svcColl);

      Assert.NotNull(serviceProvider);

      Exception ex = Assert.Throws<APIInternalException>(
        () => serviceProvider.GetService(typeof(IElasticClient))
      );

      Assert.Equal("No servers configured", ex.Message);
    }

    /// <summary>
    /// Get an Elasticsearch client instance with a valid configuration.
    /// </summary>
    [Fact]
    public void ConfigureServices_ElasticsearchGoodConfiguration()
    {
      string appsettings = @"
{
    ""Elasticsearch"": {
        ""Servers"": ""http://localhost:9200"",
        ""Userid"": """",
        ""Password"": """",
        ""MaximumRetries"": 5
    }
  }
      ";

      // Create an appsettings.json in a location where only this test will see it.
      string configLocation = Path.Join(Fixture.TestLocation, nameof(ConfigureServices_ElasticsearchGoodConfiguration));
      DirectoryInfo di = Directory.CreateDirectory(configLocation);
      File.WriteAllText(Path.Join(configLocation, "appsettings.json"), appsettings);

      // Customize the hosting environment for this test.
      IHostingEnvironment hostenv = new MockHostingEnvironment();
      hostenv.ContentRootPath = configLocation;

      Mock<NciStartupBase> mockStartup = new Mock<NciStartupBase>(hostenv) { CallBase = true };

      NciStartupBase startup = mockStartup.Object;

      IServiceCollection svcColl = new ServiceCollection();
      IServiceProvider serviceProvider = startup.ConfigureServices(svcColl);

      Assert.NotNull(serviceProvider);

      Object svc = serviceProvider.GetService(typeof(IElasticClient));
      Assert.NotNull(svc);
      Assert.IsAssignableFrom<IElasticClient>(svc);
    }

    /// <summary>
    /// Verify that we didn't forget to register the logging service.
    /// </summary>
    [Fact]
    public void ConfigureServices_GetLoggers()
    {
      IHostingEnvironment hostenv = new MockHostingEnvironment();

      Mock<NciStartupBase> mockStartup = new Mock<NciStartupBase>(hostenv) { CallBase = true };

      mockStartup.Protected().Setup("AddAdditionalConfigurationMappings", Moq.Protected.ItExpr.IsAny<IServiceCollection>());

      NciStartupBase startup = mockStartup.Object;

      IServiceCollection svcColl = new ServiceCollection();
      IServiceProvider serviceProvider = startup.ConfigureServices(svcColl);

      Object svc;

      svc = serviceProvider.GetService(typeof(ILogger<DefaultController>));
      Assert.NotNull(svc);
      Assert.IsAssignableFrom<Logger<DefaultController>>(svc);

      svc = serviceProvider.GetService(typeof(ILogger<ElasticClient>));
      Assert.NotNull(svc);
      Assert.IsAssignableFrom<Logger<ElasticClient>>(svc);
    }

  }
}