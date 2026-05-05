using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using HairSalonApp.Models;

namespace HairSalonApp.Services
{
    public class SalonDataManager
    {
        private readonly string _filePath = "salon_data.json";
        private readonly JsonSerializerOptions _options;

        public SalonDataManager()
        {
            _options = new JsonSerializerOptions
            {
                WriteIndented = true, 
                ReferenceHandler = ReferenceHandler.Preserve, 
                PropertyNameCaseInsensitive = true
            };
        }

        public void SaveData(HairSalon salon)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(salon, _options);
                File.WriteAllText(_filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        public HairSalon LoadData()
        {
            if (!File.Exists(_filePath))
            {
                return new HairSalon(1); 
            }

            try
            {
                string jsonString = File.ReadAllText(_filePath);
                var salon = JsonSerializer.Deserialize<HairSalon>(jsonString, _options);

                return salon ?? new HairSalon(1);
            }
            catch (Exception)
            {
                return new HairSalon(1);
            }
        }
    }
}