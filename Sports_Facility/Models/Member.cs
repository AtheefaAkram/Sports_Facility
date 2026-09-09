namespace Sports_Facility.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public required string MemberName { get; set; } 
        public required string Email { get; set; } 
        public required string PhoneNo { get; set; } 
        public string? Address { get; set; }
        public required string PasswordHash { get; set; } 
        public DateTime RegDate { get; set; } = DateTime.Now;
    }
}
