using System.ComponentModel.DataAnnotations;

namespace CORS
{
    /// <summary>
    /// Config options from the app.config with the same name
    /// </summary>
    public class CORSConfigSettings
    {
        [Required]
        public Dictionary<string, CorsPolicyOptions> Policies { get; set; } = [];
    }

    public class CorsPolicyOptions
    {
        [Required]
        public List<string> Origins { get; set; } = [];

        [Required]
        public bool AllowAnyHeader { get; set; }

        [Required]
        public bool AllowAnyMethod { get; set; }

        [Required]
        public bool AllowCredentials { get; set; }
    }
}
