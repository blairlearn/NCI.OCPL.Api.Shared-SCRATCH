using System;

using Microsoft.AspNetCore.Hosting;
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
      Mock<NciStartupBase> startup = new Mock<NciStartupBase>(HostingEnvironment.Object){ CallBase = true };

      NciStartupBase mock = startup.Object;

      Assert.NotNull(mock.Configuration);
    }
  }
}