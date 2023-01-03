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
}
