using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HairSalonApp.Models
{
    public class Hairstyle
    {
        [Required(ErrorMessage = "Name of hairstyle is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
        public string Name { get; set; }

        public ClientType ClientCategory { get; set; }

        [Required(ErrorMessage = "Hairdresser must be assigned.")]
        public Hairdresser Hairdresser { get; set; }

        [Required(ErrorMessage = "First name of hairdresser is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must be between 3 and 50 characters.")]
        public string HairdresserFirstName { get; set; }

        [Required(ErrorMessage = "Last name of hairdresser is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Last name must be between 3 and 50 characters.")]
        public string HairdresserLastName { get; set; }


        [Range(50, 10000, ErrorMessage = "Hairstyle price must be between 50 and 10000 UAH.")]
        public int Price { get; set; }

        public bool NeedsAdditionalServices { get; set; }

        [JsonConstructor]
        public Hairstyle()
        {
        }

        public Hairstyle(string name, ClientType clientCategory, Hairdresser hairdresser, int price, bool needsAdditionalServices)
        {
            Name = name;
            ClientCategory = clientCategory;
            Hairdresser = hairdresser;
            Price = price;
            NeedsAdditionalServices = needsAdditionalServices;
        }

        public override string ToString()
        {
            string addServices = NeedsAdditionalServices ? "Yes" : "No";
            return $"Hairstyle: {Name}, Client: {ClientCategory}, Hairdresser: {Hairdresser.LastName}, Price: {Price} UAH, Additional services: {addServices}";
        }
    }
}