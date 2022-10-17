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

    /// <summary>
    /// Initializes a new instance of the <see cref="ExamplesController"/> class.
    /// </summary>
    /// <param name="exampleProvider">Provider to get examples from the DB.</param>
    public ExamplesController(IExampleProvider exampleProvider)
    {
        _exampleProvider = exampleProvider;
    }

    /// <summary>
    /// Retrieves and returns a list of examples.
    /// </summary>
    /// <returns>A list of examples.</returns>
    [HttpGet(nameof(Examples))]
    public ActionResult<string> Examples()
    {
        try
        {
            return Environment.GetEnvironmentVariable("TEST_VAR") + "--end";
        }
        catch (Exception e)
        {
            return e.Message;
        }

        // return Ok(_exampleProvider.Get());
    }
}
