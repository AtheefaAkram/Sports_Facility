using System.ComponentModel.DataAnnotations;

namespace Sports_Facility.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string MemberName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PhoneNo { get; set; } = string.Empty;

        public string? Address { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;

        public List<int> PreferredSportIds { get; set; } = new();

        public List<Sport>? AvailableSports { get; set; }
    }
}