using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Clamify.Web.Controllers;

/// <summary>
/// Controller defining actions for the fictional Example controller.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ExampleController
{
    /// <summary>
    /// Constructors the example controller
    /// </summary>
    public ExampleController()
    {

    }

    /// <summary>
    /// Retrieves and returns a list of examples.
    /// </summary>
    /// <returns>A list of examples.</returns>
    [HttpGet(nameof(Examples))]
    public ActionResult Examples()
    {
        return new OkObjectResult("howdy");
    }
}
