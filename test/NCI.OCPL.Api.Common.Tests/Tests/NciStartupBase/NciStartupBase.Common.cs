using System;

using Microsoft.AspNetCore.Hosting;
using Moq;
using Xunit;

namespace NCI.OCPL.Api.Common
{
  public partial class NciStartupBaseTest : IClassFixture<NciStartupBaseTestFixture>
  {
    public NciStartupBaseTest(NciStartupBaseTestFixture fixture)
    {
      this.Fixture = fixture;
    }

    /// <summary>
    /// Instance of the test fixture, avaialable for any test which needs it.
    /// </summary>
    private NciStartupBaseTestFixture Fixture {get;set;}


    /// <summary>
    /// An IHostingEnvironment instance for unit tests.
    /// </summary>
    /// <value>A mock instance of IHostingEnvironment with some default
    /// properties already set.
    /// </value>
    private Mock<IHostingEnvironment> HostingEnvironment
    {
      get
      {
        Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
        env.Setup(e => e.ApplicationName).Returns("unitTest");
        env.Setup(e => e.ContentRootPath).Returns(Environment.CurrentDirectory);
        env.Setup(e => e.EnvironmentName).Returns("unitTest");
        env.Setup(e => e.WebRootPath).Returns(Environment.CurrentDirectory);

        return env;
      }
    }
  }
}