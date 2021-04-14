using Xunit;

namespace NCI.OCPL.Api.Common
{
  public partial class APIErrorExceptionTest
  {
    /// <summary>
    /// Verify the status code and message are unchanged.
    /// </summary>
    [Theory]
    [InlineData(400, "bad request")]
    [InlineData(401, "unauthorized")]
    [InlineData(404, "not found")]
    [InlineData(409, "conflict")]
    [InlineData(500, "internal server error")]
    [InlineData(501, "not implemented")]
    public void ValuesIntact(int statusCode, string message)
    {

      try
      {
        throw new APIErrorException(statusCode, message);
      }
      catch (System.Exception ex)
      {
        APIErrorException error = ex as APIErrorException;

        Assert.NotNull(error);

        Assert.Equal(statusCode, error.HttpStatusCode);
        Assert.Equal(message, error.Message);
        Assert.Equal(message, ex.Message);
      }
    }
  }
}