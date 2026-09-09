namespace Sports_Facility.Models
{
    public class Inquiry
    {
        public int InquiryId { get; set; }

        public int GuestId { get; set; }
        public Guest? Guest { get; set; }

        public required string Message { get; set; } 
        public DateTime InquiryDate { get; set; } = DateTime.Now;
    }
}
