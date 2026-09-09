
using System.ComponentModel.DataAnnotations.Schema;

namespace Sports_Facility.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int BookingId { get; set; }
        public Booking? Booking { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal Amount { get; set; }

        public required string PaymentMethod { get; set; } 
        public string PaymentStatus { get; set; } = "Pending";
        public DateTime PayDate { get; set; } = DateTime.Now;
    }
}
