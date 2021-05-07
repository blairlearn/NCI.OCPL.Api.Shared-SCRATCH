using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;
using Moq.Protected;
using Xunit;

namespace NCI.OCPL.Api.Common
{
  public partial class NciStartupBaseTest
  {
    /// <summary>
    /// Make sure we don't remove a service that's required for our APIs.
    /// </summary>
    [Fact]
    public void ConfigureServices_MeaningfulName()
    {
      Mock<NciStartupBase> startup = new Mock<NciStartupBase>(HostingEnvironment.Object){ CallBase = true };

      startup.Protected().Setup("AddAdditionalConfigurationMappings", Moq.Protected.ItExpr.IsAny<IServiceCollection>());

      NciStartupBase mockStartup = startup.Object;

      IServiceCollection instance = new ServiceCollection();
      mockStartup.ConfigureServices(instance);

      startup.Protected().Verify("AddAdditionalConfigurationMappings", Times.Once(), Moq.Protected.ItExpr.IsAny<IServiceCollection>());
    }
  }
}