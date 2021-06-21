using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using Moq;
using Xunit;

namespace NCI.OCPL.Api.Common
{
  public partial class NciStartupBaseTest
  {
    /// <summary>
    /// Make sure we didn't break things by not setting them up
    /// in the constructor.
    /// </summary>
    [Fact]
    public void Constructor()
    {
      Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
      IConfiguration config = mockConfiguration.Object;

      Mock<NciStartupBase> mockStartup = new Mock<NciStartupBase>(config){ CallBase = true };

      NciStartupBase startup = mockStartup.Object;

      Assert.NotNull(startup.Configuration);
    }
  }
}