using HairSalonApp.DTOs;
using HairSalonApp.Models;

namespace HairSalonApp.Services
{
    public static class SalonMapper
    {
        public static HairSalonDTO ToDTO(HairSalon salon)
        {
            var dto = new HairSalonDTO
            {
                SalonNumber = salon.SalonNumber,
                CurrentDate = salon.CurrentDate,
                AdditionalServicesPrice = HairSalon.AdditionalServicesPrice
            };

            var hairdresserDtoCache = new Dictionary<Hairdresser, HairdresserDTO>();

            foreach (var h in salon.CompletedHairstyles)
            {
                if (!hairdresserDtoCache.TryGetValue(h.Hairdresser, out var hdDto))
                {
                    hdDto = new HairdresserDTO
                    {
                        FirstName = h.Hairdresser.FirstName,
                        LastName = h.Hairdresser.LastName
                    };
                    hairdresserDtoCache[h.Hairdresser] = hdDto;
                }

                dto.CompletedHairstyles.Add(new HairstyleDTO
                {
                    Name = h.Name,
                    ClientCategoryIndex = (int)h.ClientCategory,
                    Price = h.Price,
                    NeedsAdditionalServices = h.NeedsAdditionalServices,
                    Hairdresser = hdDto
                });
            }

            return dto;
        }

        public static HairSalon ToModel(HairSalonDTO dto)
        {
            HairSalon.AdditionalServicesPrice = dto.AdditionalServicesPrice;
            var salon = new HairSalon(dto.SalonNumber) { CurrentDate = dto.CurrentDate };

            var hairdresserCache = new Dictionary<string, Hairdresser>();

            foreach (var hDto in dto.CompletedHairstyles)
            {
                string key = $"{hDto.Hairdresser.FirstName}|{hDto.Hairdresser.LastName}";

                if (!hairdresserCache.TryGetValue(key, out var hairdresser))
                {
                    hairdresser = new Hairdresser(hDto.Hairdresser.FirstName, hDto.Hairdresser.LastName);
                    hairdresserCache[key] = hairdresser;
                }

                var hairstyle = new Hairstyle(
                    hDto.Name,
                    (ClientType)hDto.ClientCategoryIndex,
                    hairdresser,
                    hDto.Price,
                    hDto.NeedsAdditionalServices
                );
                salon.AddHairstyle(hairstyle);
            }
            return salon;
        }
    }
}