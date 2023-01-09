
using System.ComponentModel.DataAnnotations;

namespace Clamify.RequestHandling.Models.Requests.Users;

/// <summary>
/// A request body for creating a user.
/// </summary>
public class CreateRequest
{
    /// <summary>
    /// The email of the user to create.
    /// </summary>
    [Required]
    [StringLength(100)]
    [MinLength(1)]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The first name of the user to create.
    /// </summary>
    [Required]
    [StringLength(100)]
    [MinLength(1)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// The last name of the user to create.
    /// </summary>
    [Required]
    [StringLength(100)]
    [MinLength(1)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// The password to set for the created user.
    /// </summary>
    [Required]
    [StringLength(100)]
    [MinLength(8)]
    [DataType(DataType.Password)]
    [RegularExpression(
        "^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|" +
        "(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$")]
    public string Password { get; set; } = string.Empty;
}
