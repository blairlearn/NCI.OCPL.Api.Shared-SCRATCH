using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using NCI.OCPL.Api.Common;

namespace NCI.OCPL.Api.Common.Controllers
{
  /// <summary>
  /// Default controller, handles all unknown routes. This is a bit hacky, but without it,
  /// Kestrel won't output a Server header, which in turn causes IIS to put in one of its
  /// own with a version number, and leaking the server version number is considered a
  /// security risk. Perhaps this can be removed one day.
  /// </summary>
  [Route("/")]
  [ApiExplorerSettings(IgnoreApi = true)]
  public class DefaultController : ControllerBase
  {
    public const int INVALID_ROUTE_STATUS_CODE = 404;

    /// <summary>
    /// Handle unknown routes for all the verbs.
    /// </summary>
    [HttpDelete("{*wildcard}")]
    [HttpGet("{*wildcard}")]
    [HttpHead("{*wildcard}")]
    [HttpOptions("{*wildcard}")]
    [HttpPatch("{*wildcard}")]
    [HttpPost("{*wildcard}")]
    [HttpPut("{*wildcard}")]
    public string Error()
    {
      throw new APIErrorException(INVALID_ROUTE_STATUS_CODE, "Invalid Route.");
    }
  }
}
