using System;

using Xunit;

namespace Microsoft.Extensions.Logging.Testing
{
  public partial class NullLoggerTest
  {
    public class Foo
    {
      public int Level {get;set;}
      public Foo()
      {
        Level = 5;
      }
    }

    [Fact]
    public void NullLogger_NothingEnabled()
    {
      NullLogger logger = new NullLogger();
      using(logger.BeginScope(new Foo()))
      {
        Assert.False(logger.IsEnabled(LogLevel.Critical));
        Assert.False(logger.IsEnabled(LogLevel.Debug));
        Assert.False(logger.IsEnabled(LogLevel.Error));
        Assert.False(logger.IsEnabled(LogLevel.Information));
        Assert.False(logger.IsEnabled(LogLevel.None));
        Assert.False(logger.IsEnabled(LogLevel.Trace));
        Assert.False(logger.IsEnabled(LogLevel.Warning));
      }
    }

    [Fact]
    public void NullLogger_Instance()
    {
      Assert.NotNull(NullLogger.Instance);
    }

    [Theory]
    [InlineData(LogLevel.Critical)]
    [InlineData(LogLevel.Debug)]
    [InlineData(LogLevel.Error)]
    [InlineData(LogLevel.Information)]
    [InlineData(LogLevel.None)]
    [InlineData(LogLevel.Trace)]
    [InlineData(LogLevel.Warning)]
    public void NullLogger_Log(LogLevel level)
    {
      NullLogger logger = new NullLogger();

      logger.Log(level, new EventId(1), new Foo(), new NotImplementedException(),
      (Foo bar, Exception ex) => {
        // This should never execute!
        throw new NotImplementedException();
      });
    }
  }
}