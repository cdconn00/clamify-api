namespace Clamify.Core.Models.DTO;

/// <summary>
/// A DTO for passing login information.
/// </summary>
public class UserLoginDto
{
    /// <summary>
    /// The email of the user attempting to log in.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The password to attempt authentication with.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
