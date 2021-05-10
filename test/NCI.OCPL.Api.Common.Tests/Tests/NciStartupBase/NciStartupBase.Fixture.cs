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
    /// Directory where temporary files may be written.
    /// </summary>
    public string BaseLocation { get; set; }

    public NciStartupBaseTestFixture()
    {
      BaseLocation = Path.Join(Path.GetTempPath(), nameof(NciStartupBaseTestFixture));
    }

    public void Dispose()
    {
      Directory.Delete(BaseLocation, true);
    }
  }

}