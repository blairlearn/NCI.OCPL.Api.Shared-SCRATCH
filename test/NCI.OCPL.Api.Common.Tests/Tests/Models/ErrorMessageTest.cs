using Xunit;

namespace NCI.OCPL.Api.Common.Models
{
  public partial class ErrorMessageTest
  {
    /// <summary>
    /// Verify the <see cref="NCI.OCPL.Api.Common.ErrorMessage.Message"/> property doesn't change the value stored.
    /// </summary>
    [Fact]
    public void MessageProperty()
    {
      string input = "simple string";
      string expected = "simple string";

      ErrorMessage test = new ErrorMessage();
      test.Message = input;
      string actual = test.Message;

      Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Verify the <see cref="NCI.OCPL.Api.Common.ErrorMessage.ToString"/> method serializes
    /// the object properly.
    /// </summary>
    [Fact]
    public void ToStringTest()
    {
      string input = "the message";
      string expected = "{\"Message\":\"the message\"}";

      ErrorMessage error = new ErrorMessage();
      error.Message = input;
      string actual = error.ToString();

      Assert.Equal(expected, actual);
    }
  }
}