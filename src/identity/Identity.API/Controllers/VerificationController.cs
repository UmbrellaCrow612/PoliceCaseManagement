using Identity.API.Annotations;
using Identity.API.DTOs;
using Identity.API.Extensions;
using Identity.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.API.Controllers
{
    /// <summary>
    /// Contains all verification logic such as verify users phone number device etc
    /// </summary>
    [ApiController]
    [Route("verification")]
    public class VerificationController(IDeviceVerificationService deviceVerificationService, IDeviceService deviceService, IUserService userService, IUserVerificationService userVerificationService) : ControllerBase
    {
        private readonly IDeviceVerificationService _deviceVerificationService = deviceVerificationService;
        private readonly IDeviceService _deviceService = deviceService;
        private readonly IUserService _userService = userService;
        private readonly IUserVerificationService _userVerificationService = userVerificationService;

        [RequireDeviceInformation]
        [HttpPost("devices")]
        public async Task<IActionResult> SendDeviceVerification([FromBody] SendDeviceVerificationDto dto)
        {
            var user = await _userService.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                return NotFound();
            }

            var exists = await _deviceService.ExistsAsync(user.Id, this.ComposeDeviceInfo());
            if (!exists)
            {
                var createResult = await _deviceService.CreateAsync(user, this.ComposeDeviceInfo());
                if (!createResult.Succeeded)
                {
                    return BadRequest(createResult);
                }
            }

            var device = await _deviceService.GetDeviceAsync(user.Id, this.ComposeDeviceInfo());
            if (device is null)
            {
                return NotFound();
            }

            var result = await _deviceVerificationService.SendVerification(user, device);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }

        [RequireDeviceInformation]
        [HttpPost("verify/devices")]
        public async Task<IActionResult> VerifyDevice([FromBody] VerifyDeviceDto dto)
        {
            var user = await _userService.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                return NotFound();
            }

            var device = await _deviceService.GetDeviceAsync(user.Id, this.ComposeDeviceInfo());
            if (device is null)
            {
                return NotFound();
            }

            var result = await _deviceVerificationService.Verify(device, dto.Code);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }

        [HttpPost("emails")]
        public async Task<IActionResult> SendEmailVerification([FromBody] SendEmailVerificationDto dto)
        {
            var user = await _userService.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                return NotFound();
            }

            var result = await _userVerificationService.SendEmailVerification(user);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }

        [HttpPost("verify/emails")]
        public async Task<IActionResult> VerifyEmailWithCode([FromBody] VerifyEmailDto dto)
        {
            var user = await _userService.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                return NotFound();
            }

            var result = await _userVerificationService.VerifyEmail(user, dto.Code);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }

        [HttpPost("phone-numbers")]
        public async Task<IActionResult> SendPhoneVerification([FromBody] SendPhoneNumberVerificationDto dto)
        {
            var user = await _userService.FindByPhoneNumberAsync(dto.PhoneNumber);
            if (user is null)
            {
                return NotFound();
            }

            var result = await _userVerificationService.SendPhoneVerification(user);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }

        [HttpPost("verify/phone-numbers")]
        public async Task<IActionResult> VerifyPhoneNumber([FromBody] VerifyPhoneNumberDto dto)
        {
            var user = await _userService.FindByPhoneNumberAsync(dto.PhoneNumber);
            if (user is null)
            {
                return NotFound();
            }

            var result = await _userVerificationService.VerifyPhoneNumber(user, dto.Code);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }

        [Authorize]
        [HttpPost("verify/totp")]
        public async Task<IActionResult> VerifyTotp([FromBody]VerifyTotpDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return BadRequest("User id missing");

            var user = await _userService.FindByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }

            var result = await _userVerificationService.VerifyTotp(user, dto.Code);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }
    }
}
