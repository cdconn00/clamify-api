using Clamify.Core.Providers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clamify.Web.Controllers;

/// <summary>
/// Controller defining actions for the fictional Example controller.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ExamplesController : ControllerBase
{
    private readonly IExampleProvider _exampleProvider;
    private readonly ILogger<ExamplesController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExamplesController"/> class.
    /// </summary>
    /// <param name="exampleProvider">Provider to get examples from the DB.</param>
    /// <param name="logger">Logger to log information.</param>
    public ExamplesController(IExampleProvider exampleProvider, ILogger<ExamplesController> logger)
    {
        _exampleProvider = exampleProvider;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves and returns a list of examples.
    /// </summary>
    /// <returns>A list of examples.</returns>
    [HttpGet(nameof(Examples))]
    public ActionResult Examples()
    {
        _logger.LogInformation("Getting examples...");
        return Ok(_exampleProvider.Get());
    }
}
