using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HairSalonApp.Models
{
    public class Hairdresser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [JsonConstructor]
        public Hairdresser()
        {
        }

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