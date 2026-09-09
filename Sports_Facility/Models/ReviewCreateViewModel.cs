using System.ComponentModel.DataAnnotations;

namespace Sports_Facility.Models
{
    public class ReviewCreateViewModel
    {
        public int BookingId { get; set; }
        public string? FacilitySummary { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }
    }
}