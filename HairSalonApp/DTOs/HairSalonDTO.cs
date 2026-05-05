namespace HairSalonApp.DTOs
{
    public class HairSalonDTO
    {
        public int SalonNumber { get; set; }
        public DateTime CurrentDate { get; set; }
        public int AdditionalServicesPrice { get; set; }
        public List<HairstyleDTO> CompletedHairstyles { get; set; } = new List<HairstyleDTO>();
    }
}