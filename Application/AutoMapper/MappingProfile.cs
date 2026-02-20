using AutoMapper;
using AsyncWarehouse.Domain.Models;
using AsyncWarehouse.Application.DTOs.CreateUpdateDTOs;
using AsyncWarehouse.Domain.Enums;

namespace AsyncWarehouse.Application.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ElectronicsCreateUpdateDto, Electronics>();
        //CreateMap<GetElectronics, Electronics>();

        CreateMap<FurnitureCreateUpdateDto, Furniture>();

        CreateMap<FurnitureCreateUpdateDto, Dimension>()
            .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.Length))
            .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height));
        
        CreateMap<ChemicalsCreateUpdateDto, Chemicals>()
            .ForMember(dest => dest.Hazard, opt => opt.MapFrom(src => (HazardClass)src.HazardClass));
    }
}