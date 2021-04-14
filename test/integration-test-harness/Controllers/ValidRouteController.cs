using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using NCI.OCPL.Api.Common;

namespace NCI.OCPL.Api.Common.Controllers
{
  /// <summary>
  /// Controller for testing that the default route doesn't break known routes.
  /// </summary>
  [Route("valid-route")]
  [ApiExplorerSettings(IgnoreApi = true)]
  public class ValidRouteController : ControllerBase
  {
    public const int INVALID_ROUTE_STATUS_CODE = 404;

    /// <summary>
    /// Handle unknown routes for all the verbs.
    /// </summary>
    [HttpDelete]
    [HttpGet]
    [HttpHead]
    [HttpOptions]
    [HttpPatch]
    [HttpPost]
    [HttpPut]
    public string Error()
    {
      return "Valid Route.";
    }
  }
}
