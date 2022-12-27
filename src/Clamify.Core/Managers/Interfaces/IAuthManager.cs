using Clamify.Core.Models;
using Clamify.Core.Models.DTO;
using Clamify.Entities;

namespace Clamify.Core.Managers.Interfaces;

/// <summary>
/// Contract defining methods to facilitate auth of users.
/// </summary>
public interface IAuthManager
{
    /// <summary>
    /// Method attempts to authenticate a user.
    /// </summary>
    /// <param name="userDto">The object containing credentials to authenticate with.</param>
    /// <returns>If the user was authenticated successfully with user payload if successful. </returns>
    public Task<UserAuthenticationResult> TryAuthenticateUser(UserLoginDto userDto);

    /// <summary>
    /// Creates a token for an authenticated user.
    /// </summary>
    /// <param name="user">The user to issue the token for.</param>
    /// <returns>A JWT to be issued.</returns>
    public Task<string> IssueToken(ClamifyUser user);
}
