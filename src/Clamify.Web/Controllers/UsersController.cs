using System.Net;
using Clamify.Core.Managers.Interfaces;
using Clamify.Core.Providers.Interfaces;
using Clamify.Core.Writers.Interfaces;
using Clamify.Entities;
using Clamify.RequestHandling.Models.Requests.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Clamify.Web.Controllers;

/// <summary>
/// Contains endpoints to interact with the user object.
/// </summary>
public class UsersController : Controller
{
    private readonly UserManager<ClamifyUser> _userManager;
    private readonly ILogger<UsersController> _logger;
    private readonly IFeatureFlagProvider _featureFlagProvider;
    private readonly IMessageWriter messageWriter;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="userManager">Provides access to user objects.</param>
    /// <param name="logger">Provides methods for logging errors.</param>
    /// <param name="featureFlagProvider">Provides methods for determining if a feature is enabled.</param>
    /// <param name="messageWriter">Allows an email to be sent to a user.</param>
    public UsersController(
        UserManager<ClamifyUser> userManager,
        ILogger<UsersController> logger,
        IFeatureFlagProvider featureFlagProvider,
        IMessageWriter messageWriter)
    {
        _userManager = userManager;
        _logger = logger;
        _featureFlagProvider = featureFlagProvider;
        this.messageWriter = messageWriter;
    }

    /// <summary>
    /// Registers a new user for the service and sends them a email verification code.
    /// </summary>
    /// <param name="request">The body of the create request.</param>
    /// <returns>The result of the creation request.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Create([FromBody] CreateRequest request)
    {
        throw new NotImplementedException();
    }
}
