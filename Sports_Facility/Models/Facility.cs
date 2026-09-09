using System.ComponentModel.DataAnnotations.Schema;

namespace Sports_Facility.Models
{
    public class Facility
    {
        public int FacilityId { get; set; }
        public required string FacilityName { get; set; } 
        public string? Description { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal HourlyRate { get; set; }

        // Foreign key to Location
        public int LocationId { get; set; }
        public Location? Location { get; set; }

        // Foreign key to Sport
        public int SportId { get; set; }
        public Sport? Sport { get; set; }
    }
}
