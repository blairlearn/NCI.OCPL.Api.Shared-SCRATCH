using System;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Moq;
using Xunit;

namespace NCI.OCPL.Api.Common
{
  public partial class NciStartupBaseTest : IClassFixture<NciStartupBaseTestFixture>
  {
    /// <summary>
    /// Replacement for any place an instance of IHostingEnvironment is required.
    /// Ideally, this wouldn't be necessary and we could just mock the entire object,
    /// however some methods (notably, IsDevelopment()) are extension methods and cannot
    /// be mocked.
    /// </summary>
    private class MockHostingEnvironment : IHostingEnvironment
    {
      public string ApplicationName{get;set;} = "unitTest";
      public string ContentRootPath{get;set;} = Environment.CurrentDirectory;
      public string EnvironmentName{get;set;} = "unitTest";
      public string WebRootPath{get;set;} = Environment.CurrentDirectory;
      public IFileProvider WebRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public IFileProvider ContentRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

      private bool isDevelopment = false;
      public bool IsDevelopment() => isDevelopment;
      public void SetIsDevelopment(bool value) => isDevelopment = value;
    }

    public NciStartupBaseTest(NciStartupBaseTestFixture fixture)
    {
      this.Fixture = fixture;
    }

    /// <summary>
    /// Instance of the test fixture, avaialable for any test which needs it.
    /// </summary>
    private NciStartupBaseTestFixture Fixture {get;set;}
  }
}