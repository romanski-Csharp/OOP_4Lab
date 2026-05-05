using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using HairSalonApp.Models;
using HairSalonApp.DTOs;

namespace HairSalonApp.Services
{
    public class SalonDataManager
    {
        private readonly string _filePath = "salon_data.json";
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions { WriteIndented = true };

        public void SaveData(HairSalon salon)
        {
            try
            {
                var dto = SalonMapper.ToDTO(salon);
                string jsonString = JsonSerializer.Serialize(dto, _options);
                File.WriteAllText(_filePath, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving: {ex.Message}");
            }
        }

        public HairSalon LoadData()
        {
            if (!File.Exists(_filePath)) return new HairSalon(1);
            try
            {
                string jsonString = File.ReadAllText(_filePath);
                var dto = JsonSerializer.Deserialize<HairSalonDTO>(jsonString, _options);
                return dto != null ? SalonMapper.ToModel(dto) : new HairSalon(1);
            }
            catch (Exception)
            {
                return new HairSalon(1);
            }
        }
    }
}