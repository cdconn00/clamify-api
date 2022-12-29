using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Castle.Core.Resource;
using Clamify.Core.Managers;
using Clamify.Core.Managers.Interfaces;
using Clamify.Core.Models;
using Clamify.Core.Models.DTO;
using Clamify.Entities;
using Clamify.Tests.Mocks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Clamify.Tests.Core.Managers;

/// <summary>
/// Unit test class that verifies <see cref="AuthManager"/> methods.
/// </summary>
[TestClass]
public class AuthManagerTests
{
    private Mock<UserManager<ClamifyUser>> _userManager;
    private Mock<IConfiguration> _configuration;

    private IAuthManager GetManager =>
        new AuthManager(_userManager.Object, _configuration.Object);

    /// <summary>
    /// Initialize the tests with a mock objects.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _userManager = IdentityMockHelpers.MockUserManager<ClamifyUser>();
        _configuration = new Mock<IConfiguration>();
    }

    /// <summary>
    /// Unit test verifies a non-existent user does not pass validation.
    /// </summary>
    [TestMethod]
    public void TryAuthenticateUser_NoUser_ReturnsNotAuthenticated()
    {
        _userManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult<ClamifyUser>(null));

        GetManager.TryAuthenticateUser(new UserLoginDto())
            .Result
            .Should()
            .BeEquivalentTo(new UserAuthenticationResult
            {
                AuthenticatedUser = null,
                UserAuthenticationSuccessful = false,
            });
    }

    /// <summary>
    /// Unit test verifies a user with a bad password does not pass validation.
    /// </summary>
    [TestMethod]
    public void TryAuthenticateUser_BadPassword_ReturnsNotAuthenticated()
    {
        _userManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(new ClamifyUser()));
        _userManager.Setup(m => m.CheckPasswordAsync(It.IsAny<ClamifyUser>(), It.IsAny<string>())).Returns(Task.FromResult(false));

        GetManager.TryAuthenticateUser(new UserLoginDto())
            .Result
            .Should()
            .BeEquivalentTo(new UserAuthenticationResult
            {
                AuthenticatedUser = null,
                UserAuthenticationSuccessful = false,
            });
    }

    /// <summary>
    /// Unit test verifies a user with valid credentials passes validation.
    /// </summary>
    [TestMethod]
    public void TryAuthenticateUser_GoodCredentials_ReturnsAuthenticated()
    {
        ClamifyUser user = new ();

        _userManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
        _userManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
        _userManager.Setup(m => m.CheckPasswordAsync(It.IsAny<ClamifyUser>(), It.IsAny<string>())).Returns(Task.FromResult(true));

        GetManager.TryAuthenticateUser(new UserLoginDto())
            .Result
            .Should()
            .BeEquivalentTo(new UserAuthenticationResult
            {
                AuthenticatedUser = user,
                UserAuthenticationSuccessful = true,
            });
    }

    /// <summary>
    /// Unit test verifies a valid user can have a token generated.
    /// </summary>
    [TestMethod]
    public void IssueToken_GivenValidUser_GeneratesCorrectToken()
    {
        IList<string> roles = new List<string>();
        string jwtKey = Guid.NewGuid().ToString();
        string jwtIss = "clamify.tests";
        string lifetime = "60";

        Environment.SetEnvironmentVariable("JWT_KEY", jwtKey);
        Environment.SetEnvironmentVariable("JWT_ISS", jwtIss);
        Environment.SetEnvironmentVariable("JWT_LIFETIME", lifetime);

        ClamifyUser u = new ()
        {
            UserName = "j.doe",
            FirstName = "John",
            LastName = "Doe",
            Email = "name@test.com",
            Id = 1,
        };

        _userManager.Setup(m => m.GetRolesAsync(It.IsAny<ClamifyUser>())).Returns(Task.FromResult(roles));

        // Get issued token and read it out.
        string token = GetManager.IssueToken(u).Result;
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);

        DateTimeOffset issueTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(jwtSecurityToken.Claims.Where(x => x.Type == "nbf").First().Value));
        DateTimeOffset expirationDate = issueTime.AddMinutes(Convert.ToInt32(lifetime));

        jwtSecurityToken
            .Claims
            .Should()
            .Satisfy(
                c => c.Type == "first_name" && c.Value == u.FirstName,
                c => c.Type == "last_name" && c.Value == u.LastName,
                c => c.Type == "email" && c.Value == u.Email,
                c => c.Type == "iss" && c.Value == jwtIss,
                c => c.Type == "exp" && c.Value == expirationDate.ToUnixTimeSeconds().ToString(),
                c => c.Type == "nbf" && c.Value == issueTime.ToUnixTimeSeconds().ToString(),
                c => c.Type == "sub" && c.Value == u.Id.ToString());
    }
}
