using Identity.Application.Codes;
using Identity.Application.Helpers;
using Identity.Application.Implementations;
using Identity.Application.Settings;
using Identity.Core.Models;
using Identity.Core.Services;
using Identity.Core.ValueObjects;
using Identity.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Identity.Application.Tests
{
    [TestClass]
    public class AuthServiceTests
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

        /// <summary>
        /// User has email and phone number confirmed - created at the beginning 
        /// </summary>
        private ApplicationUser _priv_user = new()
        {
            Email = "test@gmail.com",
            PhoneNumber = "+1234567890",
            UserName = "Test",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };
        private string _priv_user_password = "Password@123";
        private DeviceInfo _priv_user_device_info = new DeviceInfo
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
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            _options = new DbContextOptionsBuilder<IdentityApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            _dbContext = new IdentityApplicationDbContext(_options);
            _dbContext.Database.EnsureCreated();

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


            // set up test user and there device

            var userCreation = await _userService.CreateAsync(_priv_user, _priv_user_password);
            Assert.IsTrue(userCreation.Succeeded);
            var deviceRes = await _deviceService.CreateAsync(_priv_user, _priv_user_device_info);
            Assert.IsTrue(deviceRes.Succeeded);
            var device = await _deviceService.GetDeviceAsync(_priv_user.Id, _priv_user_device_info);
            device!.MarkTrusted();
            _dbContext.Devices.Update(device);
            await _dbContext.SaveChangesAsync();
        }

        [TestCleanup]
        public void Cleanup()
        {
          _dbContext.Dispose();
        }

        // Tests

        #region LoginAsync Tests

        [TestMethod]
        public async Task LoginAsync_WithNonExistentUser_ShouldFail()
        {
            // Act
            var result = await _authService.LoginAsync("nonexistent@user.com", "Password@123", new DeviceInfo { DeviceFingerPrint = "123", IpAddress = "123", UserAgent = "123"});

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.IsTrue(result.Errors.Any(e => e.Code == BusinessRuleCodes.IncorrectCredentials));
        }

        [TestMethod]
        public async Task LoginAsync_WithIncorrectPassword_ShouldFail()
        {
            // Act
            var result = await _authService.LoginAsync(_priv_user.Email, "WrongPassword!", _priv_user_device_info);

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.IsTrue(result.Errors.Any(e => e.Code == BusinessRuleCodes.IncorrectCredentials));
        }

        [TestMethod]
        public async Task LoginAsync_WithUnconfirmedEmail_ShouldFail()
        {
            // Arrange
            var testUser = new ApplicationUser
            {
                Email = "nefinn@gmail.com",
                PhoneNumber = "+484848484",
                UserName = "ionfoenfinef12",
            };
            var res = await _userService.CreateAsync(testUser, "Password@123");
            Assert.IsTrue(res.Succeeded);

            // Act
            var result = await _authService.LoginAsync(testUser.Email, "Password@123", new DeviceInfo { DeviceFingerPrint = "123", IpAddress="efef", UserAgent = "efef"});

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
                Email = "cscsdcsc@gmail.com",
                PhoneNumber = "+484322332848484",
                UserName = "wevfersdcfv4332",
                EmailConfirmed = true
            };
            var res = await _userService.CreateAsync(testUser, "Password@123");
            Assert.IsTrue(res.Succeeded);

            // Act
            var result = await _authService.LoginAsync(testUser.Email, "Password@123", new DeviceInfo { DeviceFingerPrint = "123", IpAddress = "efef", UserAgent = "efef" });

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
                Email = "csc212121sdcsc@gmail.com",
                PhoneNumber = "+484322343248484",
                UserName = "w22evfersdcfv4332",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            var res = await _userService.CreateAsync(testUser, "Password@123");
            Assert.IsTrue(res.Succeeded);

            // Act
            var result = await _authService.LoginAsync(testUser.Email, "Password@123", new DeviceInfo { DeviceFingerPrint=Guid.NewGuid().ToString(), IpAddress = "123", UserAgent = "123"});

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
                Email = "wqnodnqwodn21@gmail.com",
                PhoneNumber = "+902902109219021",
                UserName = "w22evf2ersdcfv4332",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            var testUserDeviceInfo = new DeviceInfo { DeviceFingerPrint = "finonfioewf", IpAddress="123", UserAgent="ienfifnefas" };
            var res = await _userService.CreateAsync(testUser, "Password@123");
            Assert.IsTrue(res.Succeeded);

            var deviceResult = await _deviceService.CreateAsync(testUser, testUserDeviceInfo);
            Assert.IsTrue(deviceResult.Succeeded);
            var device = await _deviceService.FindByIdAsync(_deviceIdentificationGenerator.GenerateId(testUser.Id, testUserDeviceInfo));
            Assert.IsNotNull(device);
            device.MarkTrusted();
            _dbContext.Devices.Update(device);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _authService.LoginAsync(testUser.Email, "Password@123", testUserDeviceInfo);

            // Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(0, result.Errors.Count);

            // Verify a login record was created in the database
            var loginRecord = await _dbContext.Logins.FindAsync(result.LoginId);
            Assert.IsNotNull(loginRecord);
            Assert.AreEqual(testUser.Id, loginRecord.UserId);
            Assert.AreEqual(device.Id, loginRecord.DeviceId);
            Assert.AreEqual(LoginStatus.TwoFactorAuthenticationReached, loginRecord.Status);
        }

        #endregion

        #region Logout Tests
        [TestMethod]
        public async Task LogoutAsync_ShouldRevokeAllValidTokensForUser()
        {
            // Arrange

            var privUserDevice = await _deviceService.GetDeviceAsync(_priv_user.Id, _priv_user_device_info);
            Assert.IsNotNull(privUserDevice);

            var tokens = new List<Token>()
            {
                new()
                {
                    DeviceId = privUserDevice.Id,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                    Id = Guid.NewGuid().ToString(),
                    UserId = _priv_user.Id
                },
                new()
                {
                    DeviceId = privUserDevice.Id,
                    ExpiresAt = DateTime.UtcNow.AddHours(1),
                    Id = Guid.NewGuid().ToString(),
                    UserId = _priv_user.Id
                },
            };
            await _dbContext.Tokens.AddRangeAsync(tokens);
            await _dbContext.SaveChangesAsync();


            // Act
            var result = await _authService.LogoutAsync(_priv_user.Id);

            // Assert
            Assert.IsTrue(result.Succeeded);

            // check there is not any valid tokens still in db for user
            var anyValidTokensExist = await _dbContext.Tokens
                .Where(x => x.ExpiresAt > DateTime.UtcNow && x.UsedAt == null && x.RevokedAt == null)
                .AnyAsync();
            Assert.IsFalse(anyValidTokensExist);
        }
        #endregion

    }
}
