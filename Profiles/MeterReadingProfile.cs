using AutoMapper;
using Ensek.DTO;
using Ensek.Models;

namespace Ensek.Profiles
{
    public class MeterReadingProfile : Profile
    {
        public MeterReadingProfile()
        {
            CreateMap<MeterReadingRead, MeterReading>();
        }
    }
}
