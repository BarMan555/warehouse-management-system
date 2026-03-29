using AutoMapper;
using WarehouseMS.Application.DTOs.CreateUpdateDTOs;
using WarehouseMS.Application.DTOs.GetDTOs;
using WarehouseMS.Domain.Enums;
using WarehouseMS.Domain.Models;

namespace WarehouseMS.Application.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ElectronicsCreateUpdateDto, Electronics>();
        CreateMap<FurnitureCreateUpdateDto, Furniture>();
        CreateMap<ChemicalsCreateUpdateDto, Chemicals>()
            .ForMember(dest => dest.Hazard, opt => opt.MapFrom(src => (HazardClass)src.HazardClass));
        
        CreateMap<FurnitureCreateUpdateDto, Dimension>()
            .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.Length))
            .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height));


        CreateMap<InventoryItem, InventoryItemGetDto>()
            .Include<Electronics, ElectronicsGetDto>()
            .Include<Chemicals, ChemicalsGetDto>()
            .Include<Furniture, FurnitureGetDto>();

        CreateMap<Electronics, ElectronicsGetDto>();

        CreateMap<Furniture, FurnitureGetDto>()
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Dimensions.Height))
            .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Dimensions.Width))
            .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.Dimensions.Length));

        CreateMap<Chemicals, ChemicalsGetDto>()
            .ForMember(dest => dest.HazardClass, opt => opt.MapFrom(src => (int)src.Hazard));
        
        CreateMap<Pallet, PalletGetDto>();
        CreateMap<PalletCreateUpdateDto, Pallet>();
    }
}