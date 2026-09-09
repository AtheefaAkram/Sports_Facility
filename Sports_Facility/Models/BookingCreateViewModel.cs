using System.ComponentModel.DataAnnotations;

namespace Sports_Facility.Models
{
    public class BookingCreateViewModel
    {
        public int FacilityId { get; set; }
        public string? FacilityName { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public string StartTime { get; set; } = string.Empty;

        [Required]
        public string EndTime { get; set; } = string.Empty;
    }
}