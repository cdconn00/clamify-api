using Clamify.Entities;

namespace Clamify.Core.Models;

/// <summary>
/// Object containing the result and payload of a user authentication attempt.
/// </summary>
public class UserAuthenticationResult
{
    private bool userAuthenticationSuccessful = false;
    private ClamifyUser? authenticatedUser;

    /// <summary>
    /// A flag determining if the authentication was successful.
    /// </summary>
    public bool UserAuthenticationSuccessful { get => userAuthenticationSuccessful; set => userAuthenticationSuccessful = value; }

    /// <summary>
    /// The authenticated user (if authentication was successful).
    /// </summary>
    public ClamifyUser? AuthenticatedUser { get => authenticatedUser; set => authenticatedUser = value; }
}
