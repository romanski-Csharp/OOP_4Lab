using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HairSalonApp.Models
{
    public class Hairdresser
    {
        [Required(ErrorMessage = "First name of hairdresser is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must be between 3 and 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name of hairdresser is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Last name must be between 3 and 50 characters.")]
        public string LastName { get; set; }

        [JsonConstructor]
        public Hairdresser() { }

        public Hairdresser(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}