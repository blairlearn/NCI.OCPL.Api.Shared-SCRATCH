using System;
using System.IO;

namespace NCI.OCPL.Api.Common
{
  /// <summary>
  /// Test fixture for the ConfigureServices tests.
  /// </summary>
  public class NciStartupBaseTestFixture : IDisposable
  {
    /// <summary>
    /// Directory where temporary files may be written. Use this when a test
    /// requires a unique location in the file system.
    /// </summary>
    public string TestLocation { get; set; }

    public NciStartupBaseTestFixture()
    {
      TestLocation = Path.Join(Path.GetTempPath(), nameof(NciStartupBaseTestFixture));
    }

    public void Dispose()
    {
      Directory.Delete(TestLocation, true);
    }
  }

}