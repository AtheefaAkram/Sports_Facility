using System.ComponentModel.DataAnnotations;
namespace Sports_Facility.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        public int BookingId { get; set; }
        public Booking? Booking { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now;
    }
}
