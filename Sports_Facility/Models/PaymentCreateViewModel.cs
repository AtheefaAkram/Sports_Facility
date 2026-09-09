using System.ComponentModel.DataAnnotations;

namespace Sports_Facility.Models
{
    public class PaymentCreateViewModel
    {
        public int BookingId { get; set; }
        public string? FacilityName { get; set; }
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;
    }
}