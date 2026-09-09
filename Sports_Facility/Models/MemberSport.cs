namespace Sports_Facility.Models
{
    public class MemberSport
    {
        public int MemberSportId { get; set; }

        public int MemberId { get; set; }
        public Member? Member { get; set; }

        public int SportId { get; set; }
        public Sport? Sport { get; set; }
    }
}
