using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.BuyingCatalogue;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Marketing.Models;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Marketing.Models.NativeDesktop;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.MappingProfiles
{
    public class NativeDesktopProfile : Profile
    {
        public NativeDesktopProfile()
        {
            CreateMap<CatalogueItem, AdditionalInformationModel>()
                .ForMember(dest => dest.BackLink,
                    opt => opt.MapFrom(src =>
                        $"/marketing/supplier/solution/{src.CatalogueItemId}/section/native-desktop"))
                .ForMember(dest => dest.AdditionalInformation, opt =>
                {
                    opt.SetMappingOrder(20);
                    opt.MapFrom((_, dest) => dest.ClientApplication?.NativeDesktopAdditionalInformation);
                })
                .IncludeBase<CatalogueItem, MarketingBaseModel>();

            CreateMap<CatalogueItem, ConnectivityModel>()
                .ForMember(dest => dest.BackLink,
                    opt => opt.MapFrom(src =>
                        $"/marketing/supplier/solution/{src.CatalogueItemId}/section/native-desktop"))
                .ForMember(dest => dest.ConnectionSpeeds, opt => opt.MapFrom(src => new List<SelectListItem>
                {
                    new () { Text = "Please select"},
                    new () { Text = "0.5Mbps", Value="0.5Mbps"},
                    new () { Text = "1Mbps", Value="1Mbps"},
                    new () { Text = "1.5Mbps", Value="1.5Mbps"},
                    new () { Text = "2Mbps", Value="2Mbps"},
                    new () { Text = "3Mbps", Value="3Mbps"},
                    new () { Text = "5Mbps", Value="5Mbps"},
                    new () { Text = "8Mbps", Value="8Mbps"},
                    new () { Text = "10Mbps", Value="10Mbps"},
                    new () { Text = "15Mbps", Value="15Mbps"},
                    new () { Text = "20Mbps", Value="20Mbps"},
                    new () { Text = "30Mbps", Value="30Mbps"},
                    new () { Text = "Higher than 30Mbps", Value="Higher than 30Mbps"}
                }))
                .ForMember(dest => dest.SelectedConnectionSpeed, opt =>
                {
                    opt.SetMappingOrder(20);
                    opt.MapFrom((_, dest) => dest.ClientApplication?.NativeDesktopMinimumConnectionSpeed);
                })
                .IncludeBase<CatalogueItem, MarketingBaseModel>();
            
            CreateMap<CatalogueItem, HardwareRequirementsModel>()
                .ForMember(dest => dest.BackLink,
                    opt => opt.MapFrom(src =>
                        $"/marketing/supplier/solution/{src.CatalogueItemId}/section/native-desktop"))
                .ForMember(dest => dest.Description, opt =>
                {
                    opt.SetMappingOrder(20);
                    opt.MapFrom((_, dest) => dest.ClientApplication?.NativeDesktopHardwareRequirements);
                })
                .IncludeBase<CatalogueItem, MarketingBaseModel>();

            CreateMap<CatalogueItem, MemoryAndStorageModel>()
                .ForMember(dest => dest.BackLink,
                    opt => opt.MapFrom(src =>
                        $"/marketing/supplier/solution/{src.CatalogueItemId}/section/native-desktop"))
                .ForMember(dest => dest.MemorySizes, opt => opt.MapFrom(src => new List<SelectListItem>
                {
                    new() { Text = "Please select"},
                    new() { Text = "256MB", Value = "256MB"},
                    new() { Text = "512MB", Value = "512MB"},
                    new() { Text = "1GB", Value = "1GB"},
                    new() { Text = "2GB", Value = "2GB"},
                    new() { Text = "4GB", Value = "4GB"},
                    new() { Text = "8GB", Value = "8GB"},
                    new() { Text = "16GB or higher", Value = "16GB or higher"}
                }))
                .ForMember(dest => dest.MinimumCpu, opt =>
                {
                    opt.SetMappingOrder(10);
                    opt.MapFrom((_, dest) =>
                        dest.ClientApplication?.NativeDesktopMemoryAndStorage?.MinimumCpu);
                })
                .ForMember(dest => dest.ScreenResolutions, opt => opt.MapFrom(src => new List<SelectListItem>
                {
                    new() { Text = "Please select", Value = "" },
                    new() { Text = "16:9 - 640 x 360", Value = "16:9 - 640 x 360" },
                    new() { Text = "4:3 - 800 x 600", Value = "4:3 - 800 x 600" },
                    new() { Text = "4:3 - 1024 x 768", Value = "4:3 - 1024 x 768" },
                    new() { Text = "16:9 - 1280 x 720", Value = "16:9 - 1280 x 720" },
                    new() { Text = "16:10 - 1280 x 800", Value = "16:10 - 1280 x 800" },
                    new() { Text = "5:4 - 1280 x 1024", Value = "5:4 - 1280 x 1024" },
                    new() { Text = "16:9 - 1360 x 768", Value = "16:9 - 1360 x 768" },
                    new() { Text = "16:9 - 1366 x 768", Value = "16:9 - 1366 x 768" },
                    new() { Text = "16:10 - 1440 x 900", Value = "16:10 - 1440 x 900" },
                    new() { Text = "16:9 - 1536 x 864", Value = "16:9 - 1536 x 864" },
                    new() { Text = "16:9 - 1600 x 900", Value = "16:9 - 1600 x 900" },
                    new() { Text = "16:10 - 1680 x 1050", Value = "16:10 - 1680 x 1050" },
                    new() { Text = "16:9 - 1920 x 1080", Value = "16:9 - 1920 x 1080" },
                    new() { Text = "16:10 - 1920 x 1200", Value = "16:10 - 1920 x 1200" },
                    new() { Text = "16:9 - 2048 x 1152", Value = "16:9 - 2048 x 1152" },
                    new() { Text = "21:9 - 2560 x 1080", Value = "21:9 - 2560 x 1080" },
                    new() { Text = "16:9 - 2560 x 1440", Value = "16:9 - 2560 x 1440" },
                    new() { Text = "21:9 - 3440 x 1440", Value = "21:9 - 3440 x 1440" },
                    new() { Text = "16:9 - 3840 x 2160", Value = "16:9 - 3840 x 2160" }
                }))
                .ForMember(dest => dest.SelectedScreenResolution, opt =>
                {
                    opt.SetMappingOrder(10);
                    opt.MapFrom((_, dest) =>
                        dest.ClientApplication?.NativeDesktopMemoryAndStorage?.RecommendedResolution);
                })
                .ForMember(dest => dest.SelectedMemorySize, opt =>
                {
                    opt.SetMappingOrder(10);
                    opt.MapFrom((_, dest) =>
                        dest.ClientApplication?.NativeDesktopMemoryAndStorage?.MinimumMemoryRequirement);
                })
                .ForMember(dest => dest.StorageDescription, opt =>
                {
                    opt.SetMappingOrder(10);
                    opt.MapFrom((_, dest) =>
                        dest.ClientApplication?.NativeDesktopMemoryAndStorage?.StorageRequirementsDescription);
                })
                .IncludeBase<CatalogueItem, MarketingBaseModel>();
            
            CreateMap<CatalogueItem, OperatingSystemsModel>()
                .ForMember(dest => dest.BackLink,
                    opt => opt.MapFrom(src =>
                        $"/marketing/supplier/solution/{src.CatalogueItemId}/section/native-desktop"))
                .ForMember(dest => dest.OperatingSystemsDescription, opt =>
                {
                    opt.SetMappingOrder(20);
                    opt.MapFrom((_, dest) => dest.ClientApplication?.NativeDesktopOperatingSystemsDescription);
                })
                .IncludeBase<CatalogueItem, MarketingBaseModel>();

            CreateMap<CatalogueItem, ThirdPartyModel>()
                .ForMember(dest => dest.BackLink,
                    opt => opt.MapFrom(src =>
                        $"/marketing/supplier/solution/{src.CatalogueItemId}/section/native-desktop"))
                .ForMember(dest => dest.DeviceCapabilities, opt =>
                {
                    opt.SetMappingOrder(20);
                    opt.MapFrom((_, dest) => dest.ClientApplication?.NativeDesktopThirdParty?.DeviceCapabilities);
                })
                .ForMember(dest => dest.ThirdPartyComponents, opt =>
                {
                    opt.SetMappingOrder(30);
                    opt.MapFrom((_, dest) => dest.ClientApplication?.NativeDesktopThirdParty?.ThirdPartyComponents);
                })
                .IncludeBase<CatalogueItem, MarketingBaseModel>();

            CreateMap<MemoryAndStorageModel, NativeDesktopMemoryAndStorage>()
                .ForMember(dest => dest.MinimumMemoryRequirement, opt => opt.MapFrom(src => src.SelectedMemorySize))
                .ForMember(dest => dest.StorageRequirementsDescription,
                    opt => opt.MapFrom(src => src.StorageDescription))
                .ForMember(dest => dest.MinimumCpu, opt => opt.MapFrom(src => src.MinimumCpu))
                .ForMember(dest => dest.RecommendedResolution, opt => opt.MapFrom(src => src.SelectedScreenResolution));
            
            CreateMap<ThirdPartyModel, NativeDesktopThirdParty>()
                .ForMember(dest => dest.DeviceCapabilities, opt => opt.MapFrom(src => src.DeviceCapabilities))
                .ForMember(dest => dest.ThirdPartyComponents, opt => opt.MapFrom(src => src.ThirdPartyComponents));
        }
    }
}