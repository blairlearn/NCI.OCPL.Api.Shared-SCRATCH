using Xunit;

namespace NCI.OCPL.Api.Common.Controllers
{
  public partial class DefaultControllerTest
  {
    /// <summary>
    /// Verify that invoking the default route's Error method fails appropriately.
    /// </summary>
    [Fact]
    public void ErrorOccurs()
    {
      DefaultController controller = new DefaultController();

      var exception = Assert.Throws<APIErrorException>(
        () => controller.Error()
      );

      Assert.Equal(DefaultController.INVALID_ROUTE_STATUS_CODE, exception.HttpStatusCode);
    }
  }
}