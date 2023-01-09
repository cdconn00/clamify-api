using Clamify.Core.Providers.Interfaces;
using Clamify.Core.Writers.Interfaces;
using Clamify.Entities;
using Clamify.Tests.Mocks;
using Clamify.Web.Controllers;
using Microsoft.AspNetCore.Identity;
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
    private IFeatureFlagProvider _featureFlagProvider;
    private IMessageWriter _messageWriter;

    private UsersController GetController =>
        new UsersController(_userManager.Object, _logger, _featureFlagProvider, _messageWriter);

    /// <summary>
    /// Initialize the tests with a mocks.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _userManager = IdentityMockHelpers.MockUserManager<ClamifyUser>();
        _logger = Mock.Of<ILogger<UsersController>>();
        _featureFlagProvider = Mock.Of<IFeatureFlagProvider>();
        _messageWriter = Mock.Of<IMessageWriter>();
    }

    /// <summary>
    /// Unit test verifies a false feature flag does not allow registrations.
    /// </summary>
    [TestMethod]
    public void Create_FeatureFlagFalse_ReturnsForbidden()
    {

    }

    /// <summary>
    /// Unit test verifies a duplicate user cannot be created.
    /// </summary>
    [TestMethod]
    public void Create_UserAlreadyExists_ReturnsBadRequest()
    {

    }

    /// <summary>
    /// Unit test verifies a <see cref="IFeatureFlagProvider"/> exceptions return a 500.
    /// </summary>
    [TestMethod]
    public void Create_ErrorInFeatureFlagProvider_ReturnsInternalServerError()
    {

    }

    /// <summary>
    /// Unit test verifies a <see cref="IMessageWriter"/> exceptions return a 500.
    /// </summary>
    [TestMethod]
    public void Create_ErrorInEmailWriter_ReturnsInternalServerError()
    {

    }

    /// <summary>
    /// Unit test verifies a <see cref="UserManager{TUser}"/> exceptions return a 500.
    /// </summary>
    [TestMethod]
    public void Create_ErrorInUserManager_ReturnsInternalServerError()
    {

    }

    /// <summary>
    /// Unit test verifies a valid request returns created.
    /// </summary>
    [TestMethod]
    public void Create_ValidRequest_ReturnsCreated()
    {

    }
}
