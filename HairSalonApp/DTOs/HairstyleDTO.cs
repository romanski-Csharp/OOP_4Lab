using System.ComponentModel.DataAnnotations;

namespace HairSalonApp.DTOs
{
    public class HairstyleDTO
    {
        [Required(ErrorMessage = "Enter a name!")]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public int ClientCategoryIndex { get; set; }

        public HairdresserDTO Hairdresser { get; set; }

        [Range(50, 10000, ErrorMessage = "From 50 to 10000 UAH")]
        public int Price { get; set; }

        public bool NeedsAdditionalServices { get; set; }
    }
}