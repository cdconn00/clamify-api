using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Clamify.Core.Managers.Interfaces;
using Clamify.Core.Models;
using Clamify.Core.Models.DTO;
using Clamify.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Clamify.Core.Managers;

/// <summary>
/// Class to facilitate authentication and token issuance.
/// </summary>
public class AuthManager : IAuthManager
{
    private readonly UserManager<ClamifyUser> _userManager;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthManager"/> class.
    /// </summary>
    /// <param name="userManager">The user manager to utilize in authentication.</param>
    /// <param name="configuration">Configuration object to get configuration values.</param>
    public AuthManager(UserManager<ClamifyUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    /// <inheritdoc/>
    public async Task<string> IssueToken(ClamifyUser user)
    {
        JwtSecurityTokenHandler handler = new ();

        SigningCredentials signingCredentials = GetSigningCredentials();
        List<Claim> userClaims = await GetUserClaims(user);
        JwtSecurityToken token = GenerateToken(signingCredentials, userClaims);

        return handler.WriteToken(token);
    }

    /// <inheritdoc/>
    public async Task<UserAuthenticationResult> TryAuthenticateUser(UserLoginDto loginDto)
    {
        ClamifyUser? retrievedUser = await _userManager.FindByEmailAsync(loginDto.Email);

        if (retrievedUser != null && await _userManager.CheckPasswordAsync(retrievedUser, loginDto.Password))
        {
            return new UserAuthenticationResult
            {
                UserAuthenticationSuccessful = true,
                AuthenticatedUser = retrievedUser,
            };
        }
        else
        {
            return new UserAuthenticationResult
            {
                UserAuthenticationSuccessful = false,
            };
        }
    }

    private SigningCredentials GetSigningCredentials()
    {
        string jwtSigningKey = GetSecret("JWT_KEY");
        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKey));

        return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetUserClaims(ClamifyUser user)
    {
        List<Claim> claims = new ()
        {
            new Claim(ClaimTypes.Name, user.UserName),
        };

        var roles = await _userManager.GetRolesAsync(user);

        foreach (string role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private JwtSecurityToken GenerateToken(SigningCredentials signingCredentials, List<Claim> userClaims)
    {
        JwtSecurityToken token = new (
            issuer: GetSecret("JWT_ISS"),
            claims: userClaims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(GetSecret("JWT_LIFETIME"))),
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials);

        return token;
    }

    private string GetSecret(string secretName)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            return _configuration[secretName] ?? throw new InvalidOperationException($"{secretName} not found.");
        }
        else
        {
            return Environment.GetEnvironmentVariable(secretName) ?? throw new InvalidOperationException($"{secretName} not found.");
        }
    }
}
