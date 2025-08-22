using Identity.Application.Codes;
using Identity.Application.Helpers;
using Identity.Application.Implementations;
using Identity.Application.Settings;
using Identity.Core.Models;
using Identity.Core.Services;
using Identity.Core.ValueObjects;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Identity.Application.Tests
{
    [TestClass]
    public class AuthServiceTests : IDisposable
    {
        // other services
        private IUserService _userService;
        private IDeviceService _deviceService;
        private IdentityApplicationDbContext _dbContext;
        private DbContextOptions<IdentityApplicationDbContext> _options;
        private IUserValidationService _userValidationService;
        private IPasswordHasher _passwordHasher;
        private ITokenService _tokenService;
        private IOptions<TimeWindows> _timeWindows;
        private IDeviceIdentificationGenerator _deviceIdentificationGenerator;
        private IRoleService _roleService;
        private JwtBearerHelper _jwtBearerHelper;
        private IOptions<JwtBearerOptions> _jwtBearerOptions;
        private ApplicationUser _user = new()
        {
            Email = "test@gmail.com",
            PhoneNumber = "+1234567890",
            UserName = "Test",
        };
        private string _password = "Password@123";
        private DeviceInfo _deviceInfo = new DeviceInfo
        {
            DeviceFingerPrint = "123",
            IpAddress = "127.0.0.1",
            UserAgent = "test"
        };


        // service where testing
        private IAuthService _authService;

        [TestInitialize]
        public async Task Setup()
        {
            _options = new DbContextOptionsBuilder<IdentityApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new IdentityApplicationDbContext(_options);


            var jwtOptions = new JwtBearerOptions
            {
                Audiences = ["auth"],
                ExpiresInMinutes = 5,
                Issuer = "auth",
                Key = "ENFNWEFINEWIFNEWFNEWUIFNUIWENFUIEWNFUIEWNUIFNA",
                RefreshTokenExpiriesInMinutes = 5,
            };
            _jwtBearerOptions = Options.Create(jwtOptions);

            var timeWindows = new TimeWindows
            {
                DeviceChallengeTime = 1,
                EmailConfirmationTime = 1,
                LoginLifetime = 1,
                PhoneConfirmationTime = 1,
                TwoFactorSmsTime = 1,
            };
            _timeWindows = Options.Create(timeWindows);

            _passwordHasher = new PasswordHasherImpl();
            _userValidationService = new UserValidationServiceImpl();
            _deviceIdentificationGenerator = new DeviceIdentificationGeneratorImpl();
            _roleService = new RoleServiceImpl(_dbContext);
            _jwtBearerHelper = new JwtBearerHelper(_jwtBearerOptions);

            _userService = new UserServiceImpl(_dbContext, _userValidationService, _passwordHasher);
            _deviceService = new DeviceServiceImpl(_dbContext, _deviceIdentificationGenerator);
            _tokenService = new TokenServiceImpl(_roleService, _jwtBearerHelper, _jwtBearerOptions,_dbContext);

            _authService = new AuthServiceImpl(_userService, _deviceService, _dbContext, _tokenService,_timeWindows);


            // set up test user

            var result = await _userService.CreateAsync(_user, _password);
            if (!result.Succeeded)
            {
                throw new ApplicationException();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
          _dbContext.Dispose();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }


        // Tests

        #region LoginAsync Tests

        [TestMethod]
        public async Task LoginAsync_WithNonExistentUser_ShouldFail()
        {
            // Act
            var result = await _authService.LoginAsync("nonexistent@user.com", _password, _deviceInfo);

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.IsTrue(result.Errors.Any(e => e.Code == BusinessRuleCodes.IncorrectCredentials));
        }

        [TestMethod]
        public async Task LoginAsync_WithIncorrectPassword_ShouldFail()
        {
            // Act
            var result = await _authService.LoginAsync(_user.Email, "WrongPassword!", _deviceInfo);

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.IsTrue(result.Errors.Any(e => e.Code == BusinessRuleCodes.IncorrectCredentials));
        }

        [TestMethod]
        public async Task LoginAsync_WithUnconfirmedEmail_ShouldFail()
        {
            // Act
            var result = await _authService.LoginAsync(_user.Email, _password, _deviceInfo);

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.IsTrue(result.Errors.Any(e => e.Code == BusinessRuleCodes.EmailNotConfirmed));
        }

        [TestMethod]
        public async Task LoginAsync_WithUnconfirmedPhone_ShouldFail()
        {
            // Arrange
            var testUser = new ApplicationUser
            {
                Email = "woenfon@gmail.com",
                PhoneNumber = "+49494949494",
                UserName = "Testdwdwdwd1",
            };
            testUser.MarkEmailConfirmed();

            var userRes = await _userService.CreateAsync(testUser, "Password@123");
            if (!userRes.Succeeded)
            {
                throw new ApplicationException();
            }

            // Act
            var result = await _authService.LoginAsync(testUser.Email, "Password@123", _deviceInfo);

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.IsTrue(result.Errors.Any(e => e.Code == BusinessRuleCodes.PhoneNotConfirmed));
        }

        [TestMethod]
        public async Task LoginAsync_WithUnregisteredDevice_ShouldFail()
        {
            // Arrange
            var testUser = new ApplicationUser
            {
                Email = "wqdwdqwdqwdqwqdwdq@gmail.com",
                PhoneNumber = "+2091092190219021",
                UserName = "iueuiewbfiub1234",
            };
            testUser.MarkEmailConfirmed();
            testUser.MarkPhoneNumberConfirmed();

            var userRes = await _userService.CreateAsync(testUser, "Password@123");
            if (!userRes.Succeeded)
            {
                throw new ApplicationException();
            }

            // Act
            var result = await _authService.LoginAsync(testUser.Email, "Password@123", _deviceInfo);

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.IsTrue(result.Errors.Any(e => e.Code == BusinessRuleCodes.Device));
        }


        [TestMethod]
        public async Task LoginAsync_WithValidCredentialsAndTrustedDevice_ShouldSucceed()
        {
            // Arrange
            var testUser = new ApplicationUser
            {
                Email = "oiwebfuiew3bfiubw@gmail.com",
                PhoneNumber = "+903209230923",
                UserName = "oeinoiwnfonw323",
            };
            testUser.MarkEmailConfirmed();
            testUser.MarkPhoneNumberConfirmed();

            var userRes = await _userService.CreateAsync(testUser, "Password@123");
            if (!userRes.Succeeded)
            {
                throw new ApplicationException();
            }

            var testDeviceInfo = new DeviceInfo
            {
                DeviceFingerPrint = "3r4r34r",
                IpAddress = "127.0.0.1",
                UserAgent = "okok"
            };

            var deviceResult = await _deviceService.CreateAsync(testUser, testDeviceInfo);
            if (!deviceResult.Succeeded)
            {
                throw new ApplicationException();
            }

            var testDevice = await _deviceService.GetDeviceAsync(testUser.Id, testDeviceInfo);
            if (testDevice is null)
            {
                throw new ApplicationException();
            }
            testDevice.MarkTrusted();
            _dbContext.Devices.Update(testDevice);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _authService.LoginAsync(testUser.Email, "Password@123", testDeviceInfo);

            // Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(0, result.Errors.Count);

            // Verify a login record was created in the database
            var loginRecord = await _dbContext.Logins.FindAsync(result.LoginId);
            Assert.IsNotNull(loginRecord);
            Assert.AreEqual(testUser.Id, loginRecord.UserId);
            Assert.AreEqual(testDevice.Id, loginRecord.DeviceId);
            Assert.AreEqual(LoginStatus.TwoFactorAuthenticationReached, loginRecord.Status);
        }

        #endregion

    }
}
