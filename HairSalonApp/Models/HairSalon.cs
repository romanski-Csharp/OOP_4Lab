using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HairSalonApp.Models
{
    public class HairSalon
    {
        [Range(1, int.MaxValue, ErrorMessage = "Salon number must be greater than 0.")]
        public int SalonNumber { get; set; }

        public DateTime CurrentDate { get; set; }

        [Range(0, 5000, ErrorMessage = "The price of additional services cannot be negative or too high.")]
        public static int AdditionalServicesPrice { get; set; } = 150;

        public ObservableCollection<Hairstyle> CompletedHairstyles { get; set; }

        [JsonConstructor]
        public HairSalon()
        {
            CompletedHairstyles = new ObservableCollection<Hairstyle>();
        }

        public HairSalon(int salonNumber)
        {
            if (salonNumber <= 0)
            {
                throw new ArgumentException("Salon number must be a positive integer.", nameof(salonNumber));
            }

            SalonNumber = salonNumber;
            CurrentDate = DateTime.Now.Date;
            CompletedHairstyles = new ObservableCollection<Hairstyle>();
        }

        public void AddHairstyle(Hairstyle hairstyle)
        {
            if (hairstyle == null)
                throw new ArgumentNullException(nameof(hairstyle), "Hairstyle cannot be null.");

            CompletedHairstyles.Add(hairstyle);
        }

        public int GetTotalDailyRevenue()
        {
            int total = CompletedHairstyles.Sum(h => h.Price);
            int additionalCosts = CompletedHairstyles.Count(h => h.NeedsAdditionalServices) * AdditionalServicesPrice;
            return total + additionalCosts;
        }

        public override string ToString()
        {
            return $"Hair Salon №{SalonNumber}, Date: {CurrentDate.ToShortDateString()}, Completed Hairstyles: {CompletedHairstyles.Count}, Revenue: {GetTotalDailyRevenue()} UAH";
        }

        public string ToShortString()
        {
            return $"№{SalonNumber} | {CurrentDate.ToShortDateString()} | Total Revenue: {GetTotalDailyRevenue()} UAH";
        }
    }
}