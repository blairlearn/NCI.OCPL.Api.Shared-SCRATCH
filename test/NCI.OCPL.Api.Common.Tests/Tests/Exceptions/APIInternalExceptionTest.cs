using Xunit;

namespace NCI.OCPL.Api.Common.Models
{
  public partial class APIInternalExceptionTest
  {
    /// <summary>
    /// Verify the exception message is unchanged.
    /// </summary>
    [Fact]
    public void MessageIntact()
    {
      string theMessage = "the message";
      try
      {
        throw new APIInternalException(theMessage);
      }
      catch (System.Exception ex)
      {
        Assert.Equal(theMessage, ex.Message);
      }
    }
  }
}