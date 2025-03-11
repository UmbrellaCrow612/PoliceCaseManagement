using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Settings
{
    /// <summary>
    /// Class that stores the app config field for the same name - used to read said value anywhere in the app using DI
    /// when needed - validation done upon it, so we dont have null checks needed.
    /// </summary>
    public class PasswordConfigSettings
    {
        /// <summary>
        /// Numbered field amount of days
        /// </summary>
        [Required]
        [Range(1,30, ErrorMessage = "Validation failed on RoationPeriodInDays needs to be between 1 and 30")]
        public required int RoationPeriodInDays { get; set; }
    }
}
