using System.ComponentModel.DataAnnotations;

namespace HairSalonApp.Models
{
    public class Hairdresser
    {
        private const string pattern = @"^[a-zA-Zа-яА-ЯіІїЇєЄґҐ\'\- ]+$";

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must be between 3 and 50 characters.")]
        [RegularExpression(pattern, ErrorMessage = "First name cannot contain numbers or special symbols.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Last name must be between 3 and 50 characters.")]
        [RegularExpression(pattern, ErrorMessage = "Last name cannot contain numbers or special symbols.")]
        public string LastName { get; set; }

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