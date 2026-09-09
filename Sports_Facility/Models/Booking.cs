namespace Sports_Facility.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int MemberId { get; set; }
        public Member? Member { get; set; }

        public int FacilityId { get; set; }
        public Facility? Facility { get; set; }

        public DateTime BookingDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string BookingStatus { get; set; } = "Pending";

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
