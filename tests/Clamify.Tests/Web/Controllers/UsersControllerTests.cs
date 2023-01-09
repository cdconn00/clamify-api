using Clamify.Core.Providers.Interfaces;
using Clamify.Core.Writers.Interfaces;
using Clamify.Entities;
using Clamify.Entities.Context;
using Clamify.RequestHandling.Models.Requests.Users;
using Clamify.Tests.Mocks;
using Clamify.Web.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Clamify.Tests.Web.Controllers;

/// <summary>
/// Unit test class verifies <see cref="UsersController"/> endpoints.
/// </summary>
[TestClass]
public class UsersControllerTests
{
    private Mock<UserManager<ClamifyUser>> _userManager;
    private ILogger<UsersController> _logger;
    private Mock<IFeatureFlagProvider> _featureFlagProvider;
    private Mock<IMessageWriter> _messageWriter;
    private ClamifyContext _context;

    private UsersController GetController =>
        new UsersController(_userManager.Object, _logger, _featureFlagProvider.Object, _messageWriter.Object);

    /// <summary>
    /// Initialize the tests with a mocks.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _userManager = IdentityMockHelpers.MockUserManager<ClamifyUser>();
        _logger = Mock.Of<ILogger<UsersController>>();
        _featureFlagProvider = new Mock<IFeatureFlagProvider>();
        _messageWriter = new Mock<IMessageWriter>();
        _context = MockClamifyContextFactory.GenerateMockContext();
    }

    /// <summary>
    /// Unit test verifies a false feature flag does not allow registrations.
    /// </summary>
    [TestMethod]
    public void Create_FeatureFlagFalse_ReturnsForbidden()
    {
        _context.FeatureFlags.Add(new FeatureFlag { FeatureFlagId = 1, FeatureName = "Registration", IsEnabled = false });
        _context.SaveChanges();

        var result = GetController.Create(GetRequest()).Result;
        result.Should().BeOfType<ForbidResult>();

        _context.Users.Should().HaveCount(0);
    }

    /// <summary>
    /// Unit test verifies a duplicate user cannot be created.
    /// </summary>
    [TestMethod]
    public void Create_UserAlreadyExists_ReturnsBadRequest()
    {
        AddTrueFeatureFlag();

        _context.Users.Add(new ClamifyUser
        {
            Id = 1,
            FirstName = "Test",
            LastName = "Test",
            Email = "test@john.com",
            UserName = "Test",
        });

        _context.SaveChanges();

        var result = GetController.Create(GetRequest()).Result;
        result.Should().BeOfType<BadRequestResult>();

        _context.Users.Should().HaveCount(1);
    }

    /// <summary>
    /// Unit test verifies a <see cref="IFeatureFlagProvider"/> exceptions return a 500.
    /// </summary>
    [TestMethod]
    public void Create_ErrorInFeatureFlagProvider_ReturnsInternalServerError()
    {
        AddTrueFeatureFlag();

        _featureFlagProvider
            .Setup(x => x.IsFeatureEnabled(It.IsAny<string>()))
            .ThrowsAsync(new Exception());

        StatusCodeResult result = (StatusCodeResult)GetController.Create(GetRequest()).Result;

        result.StatusCode.Equals(StatusCodes.Status500InternalServerError);
        _context.Users.Should().HaveCount(0);
    }

    /// <summary>
    /// Unit test verifies a <see cref="IMessageWriter"/> exceptions return a 500.
    /// </summary>
    [TestMethod]
    public void Create_ErrorInEmailWriter_ReturnsInternalServerError()
    {
        AddTrueFeatureFlag();

        _messageWriter
            .Setup(x =>
                x.SendMessage(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
            .ThrowsAsync(new Exception());

        StatusCodeResult result = (StatusCodeResult)GetController.Create(GetRequest()).Result;

        result.StatusCode.Equals(StatusCodes.Status500InternalServerError);
        _context.Users.Should().HaveCount(0);
    }

    /// <summary>
    /// Unit test verifies a <see cref="UserManager{TUser}"/> exceptions return a 500.
    /// </summary>
    [TestMethod]
    public void Create_ErrorInUserManager_ReturnsInternalServerError()
    {
        AddTrueFeatureFlag();

        _userManager
            .Setup(x => x.CreateAsync(It.IsAny<ClamifyUser>()))
            .ThrowsAsync(new Exception());

        StatusCodeResult result = (StatusCodeResult)GetController.Create(GetRequest()).Result;

        result.StatusCode.Equals(StatusCodes.Status500InternalServerError);
        _context.Users.Should().HaveCount(0);
    }

    /// <summary>
    /// Unit test verifies a valid request returns created.
    /// </summary>
    [TestMethod]
    public void Create_ValidRequest_ReturnsCreated()
    {
        AddTrueFeatureFlag();

        var result = GetController.Create(GetRequest()).Result;
        result.Should().BeOfType<CreatedResult>();

        _context.Users.Should().HaveCount(1);
    }

    private static CreateRequest GetRequest()
    {
        return new ()
        {
            Email = "test@john.com",
            FirstName = "john",
            LastName = "doe",
            Password = "Pa$$word123!!",
        };
    }

    private void AddTrueFeatureFlag()
    {
        _context.FeatureFlags.Add(new FeatureFlag { FeatureFlagId = 1, FeatureName = "Registration", IsEnabled = true });
        _context.SaveChanges();
    }
}
