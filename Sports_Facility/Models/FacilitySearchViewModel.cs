namespace Sports_Facility.Models
{
    public class FacilitySearchViewModel
    {
        public int? SportId { get; set; }
        public int? LocationId { get; set; }
        public DateTime? Date { get; set; }
        public string? Time { get; set; }

        public List<Sport> Sports { get; set; } = new();
        public List<Location> Locations { get; set; } = new();
        public List<Facility> Results { get; set; } = new();
    }
}