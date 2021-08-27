﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Extensions;
using NHSD.GPIT.BuyingCatalogue.Framework.Constants;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Solutions.Models;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Solutions.MappingProfiles
{
    public sealed class SolutionsProfile : Profile
    {
        private const string KeyBrowserBased = "browser-based";
        private const string KeyNativeDesktop = "native-desktop";
        private const string KeyNativeMobile = "native-mobile";

        private static readonly List<Func<CatalogueItem, bool>> ShowSectionFunctions = new()
        {
            { _ => true },
            { catalogueItem => catalogueItem.HasFeatures() },
            { catalogueItem => catalogueItem.HasCapabilities() },
            { catalogueItem => catalogueItem.HasListPrice() },
            { catalogueItem => catalogueItem.HasAdditionalServices() },
            { catalogueItem => catalogueItem.HasAssociatedServices() },
            { catalogueItem => catalogueItem.HasInteroperability() },
            { catalogueItem => catalogueItem.HasImplementationDetail() },
            { catalogueItem => catalogueItem.HasClientApplication() },
            { catalogueItem => catalogueItem.HasHosting() },
            { catalogueItem => catalogueItem.HasServiceLevelAgreement() },
            { catalogueItem => catalogueItem.HasDevelopmentPlans() },
            { catalogueItem => catalogueItem.HasSupplierDetails() },
        };

        public SolutionsProfile()
        {
            CreateMap<CatalogueItem, AssociatedServiceModel>()
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.AssociatedService == null ? null : src.AssociatedService.Description))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(
                    dest => dest.OrderGuidance,
                    opt => opt.MapFrom(
                        src => src.AssociatedService == null ? null : src.AssociatedService.OrderGuidance))
                .ForMember(
                    dest => dest.Prices,
                    opt =>
                    {
                        opt.PreCondition(src => src.CataloguePrices != null);
                        opt.MapFrom(src => src.CataloguePrices.Where(x => x != null && x.Price != null));
                    });

            CreateMap<CatalogueItem, AssociatedServicesModel>()
                .ForMember(
                    dest => dest.Services,
                    opt => opt.MapFrom(
                        src => src.Supplier == null ? new List<CatalogueItem>() :
                            src.Supplier.CatalogueItems == null ? new List<CatalogueItem>() :
                            src.Supplier.CatalogueItems
                                .Where(c => c.CatalogueItemType == CatalogueItemType.AssociatedService)
                                .OrderBy(c => c.Name)
                                .ToList()))
                .IncludeBase<CatalogueItem, SolutionDisplayBaseModel>()
                .AfterMap((_, dest) => dest.PaginationFooter.FullWidth = true);

            CreateMap<CatalogueItem, AdditionalServiceModel>()
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.AdditionalService == null ? null : src.AdditionalService.FullDescription))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(
                    dest => dest.Prices,
                    opt =>
                    {
                        opt.PreCondition(src => src.CataloguePrices != null);
                        opt.MapFrom(src => src.CataloguePrices.Where(x => x != null && x.Price != null));
                    })
                .ForMember(dest => dest.SolutionId, opt => opt.MapFrom(src => src.Id));

            CreateMap<CatalogueItem, AdditionalServicesModel>()
                .ForMember(
                    dest => dest.Services,
                    opt => opt.MapFrom(
                        src => src.Supplier == null ? new List<CatalogueItem>() :
                            src.Supplier.CatalogueItems == null ? new List<CatalogueItem>() :
                            src.Supplier.CatalogueItems
                                .Where(c => c.CatalogueItemType == CatalogueItemType.AdditionalService)
                                .OrderBy(c => c.Name)
                                .ToList()))
                .IncludeBase<CatalogueItem, SolutionDisplayBaseModel>()
                .AfterMap((_, dest) => dest.PaginationFooter.FullWidth = true);

            CreateMap<CatalogueItem, CapabilitiesViewModel>()
                .ForMember(dest => dest.CapabilitiesHeading, opt => opt.MapFrom(src => "Capabilities met"))
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.Ignore())
                .ForMember(
                    dest => dest.RowViewModels,
                    opt =>
                    {
                        opt.PreCondition(src => src.CatalogueItemCapabilities != null);
                        opt.MapFrom(src => src.CatalogueItemCapabilities);
                    })
                .IncludeBase<CatalogueItem, SolutionDisplayBaseModel>()
                .AfterMap((_, dest) => dest.PaginationFooter.FullWidth = true);

            CreateMap<CatalogueItem, SolutionSupplierDetailsModel>()
                .ForMember(
                    dest => dest.LastReviewed,
                    opt => opt.MapFrom<IMemberValueResolver<object, object, string, string>, string>(
                        x => "SolutionsLastReviewedDate"))
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.Supplier == null ? null : src.Supplier.Summary))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Supplier == null ? null : src.Supplier.Name))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Supplier == null ? null : src.Supplier.SupplierUrl))
                .ForMember(dest => dest.SolutionName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Contacts, opt => opt.MapFrom(src => src.Solution.MarketingContacts))
                .IncludeBase<CatalogueItem, SolutionDisplayBaseModel>();

            CreateMap<MarketingContact, SupplierContactViewModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<CatalogueItem, ClientApplicationTypesModel>()
                .ForMember(dest => dest.ApplicationTypes, opt => opt.Ignore())
                .ForMember(
                    dest => dest.BrowserBasedApplication,
                    opt => opt.MapFrom(
                        (_, dest) =>
                        ((dest.ClientApplication?.ClientApplicationTypes?.Any(x => x.EqualsIgnoreCase(KeyBrowserBased)) ?? false) ?
                        new DescriptionListViewModel
                        {
                            Heading = "Browser-based application",
                            Items = GetBrowserBasedItems(dest.ClientApplication),
                        }
                        : null)))
                .ForMember(
                    dest => dest.NativeDesktopApplication,
                    opt => opt.MapFrom(
                        (_, dest) =>
                        ((dest.ClientApplication?.ClientApplicationTypes?.Any(x => x.EqualsIgnoreCase(KeyNativeDesktop)) ?? false) ?
                        new DescriptionListViewModel
                        {
                            Heading = "Desktop application",
                            Items = GetNativeDesktopItems(dest.ClientApplication),
                        }
                        : null)))
                .ForMember(
                    dest => dest.NativeMobileApplication,
                    opt => opt.MapFrom(
                        (_, dest) =>
                        ((dest.ClientApplication?.ClientApplicationTypes?.Any(x => x.EqualsIgnoreCase(KeyNativeMobile)) ?? false) ?
                        new DescriptionListViewModel
                        {
                            Heading = "Mobile or tablet application",
                            Items = GetNativeMobileItems(dest.ClientApplication),
                        }
                        : null)))
                .IncludeBase<CatalogueItem, SolutionDisplayBaseModel>()
                .AfterMap(
                    (_, dest) =>
                    {
                        dest.PaginationFooter.FullWidth = true;

                        dest.ApplicationTypes = new DescriptionListViewModel
                        {
                            Heading = "Application type",
                            Items = new Dictionary<string, ListViewModel>(),
                        };

                        if (dest.ClientApplication?.ClientApplicationTypes?.Any(x => x.EqualsIgnoreCase(KeyBrowserBased)) ?? false)
                        {
                            dest.ApplicationTypes.Items.Add(
                                "Browser-based application",
                                new ListViewModel { Text = dest.HasApplicationType(KeyBrowserBased) });
                        }

                        if (dest.ClientApplication?.ClientApplicationTypes?.Any(x => x.EqualsIgnoreCase(KeyNativeDesktop)) ?? false)
                        {
                            dest.ApplicationTypes.Items.Add(
                                "Desktop application",
                                new ListViewModel { Text = dest.HasApplicationType(KeyNativeDesktop) });
                        }

                        if (dest.ClientApplication?.ClientApplicationTypes?.Any(x => x.EqualsIgnoreCase(KeyNativeMobile)) ?? false)
                        {
                            dest.ApplicationTypes.Items.Add(
                                "Mobile or tablet application",
                                new ListViewModel { Text = dest.HasApplicationType(KeyNativeMobile) });
                        }
                    });

            CreateMap<CatalogueItem, SolutionDisplayBaseModel>()
                .BeforeMap(
                    (src, dest) => dest.ClientApplication =
                        src.Solution != null && !string.IsNullOrEmpty(src.Solution.ClientApplication)
                            ? JsonConvert.DeserializeObject<ClientApplication>(src.Solution.ClientApplication)
                            : new ClientApplication())
                .ForMember(dest => dest.ClientApplication, opt => opt.Ignore())
                .ForMember(
                    dest => dest.LastReviewed,
                    opt => opt.MapFrom<IMemberValueResolver<object, object, string, string>, string>(
                        x => "SolutionsLastReviewedDate"))
                .ForMember(dest => dest.PaginationFooter, opt => opt.Ignore())
                .ForMember(dest => dest.SolutionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SolutionName, opt => opt.MapFrom(src => src.Name))
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .AfterMap(
                    (src, dest) =>
                    {
                        for (int i = 0; i < ShowSectionFunctions.Count; i++)
                        {
                            if (ShowSectionFunctions[i](src))
                            {
                                dest.SetShowTrue(i);
                            }
                        }

                        dest.SetPaginationFooter();
                    });

            CreateMap<CatalogueItem, ImplementationTimescalesModel>()
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.Solution == null ? null : src.Solution.ImplementationDetail))
                .IncludeBase<CatalogueItem, SolutionDisplayBaseModel>();

            CreateMap<CatalogueItemCapability, SolutionCheckEpicsModel>()
                .ForMember(dest => dest.CatalogueItemIdAdditional, opt => opt.Ignore())
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.Capability == null ? null : src.Capability.Description))
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Capability == null ? null : src.Capability.Name))
                .ForMember(dest => dest.SolutionId, opt => opt.MapFrom(src => src.CatalogueItemId))
                .ForMember(dest => dest.SolutionName, opt => opt.Ignore())
                .ForMember(
                    dest => dest.NhsDefined,
                    opt => opt.MapFrom(
                        (src) =>
                            GetEpics(src.Capability, false)))
                .ForMember(
                    dest => dest.SupplierDefined,
                    opt => opt.MapFrom(
                        (src) =>
                            GetEpics(src.Capability, true)))
                .ForMember(
                    dest => dest.LastReviewed,
                    opt => opt.MapFrom<IMemberValueResolver<object, object, string, string>, string>(
                        x => "SolutionsLastReviewedDate"));

            CreateMap<CatalogueItem, SolutionDescriptionModel>()
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.Solution == null ? null : src.Solution.FullDescription))
                .ForMember(dest => dest.Frameworks, opt => opt.MapFrom(src => src.Frameworks()))
                .ForMember(dest => dest.IsFoundation, opt => opt.MapFrom(src => src.IsFoundation().ToYesNo()))
                .ForMember(
                    dest => dest.Summary,
                    opt => opt.MapFrom(src => src.Solution == null ? null : src.Solution.Summary))
                .ForMember(
                    dest => dest.SupplierName,
                    opt => opt.MapFrom(src => src.Supplier == null ? null : src.Supplier.Name))
                .IncludeBase<CatalogueItem, SolutionDisplayBaseModel>();

            CreateMap<CatalogueItem, SolutionFeaturesModel>()
                .ForMember(dest => dest.Features, opt => opt.MapFrom(src => src.Features()))
                .IncludeBase<CatalogueItem, SolutionDisplayBaseModel>();

            CreateMap<CatalogueItem, HostingTypesModel>()
                .BeforeMap(
                (src, dest) =>
                {
                    var hosting = src.Solution.GetHosting();

                    dest.PublicCloud = hosting?.PublicCloud;
                    dest.PrivateCloud = hosting?.PrivateCloud;
                    dest.HybridHostingType = hosting?.HybridHostingType;
                    dest.OnPremise = hosting?.OnPremise;
                })
                .ForMember(dest => dest.PublicCloud, opt => opt.Ignore())
                .ForMember(dest => dest.PrivateCloud, opt => opt.Ignore())
                .ForMember(dest => dest.HybridHostingType, opt => opt.Ignore())
                .ForMember(dest => dest.OnPremise, opt => opt.Ignore())
                .IncludeBase<CatalogueItem, SolutionDisplayBaseModel>();

            CreateMap<CatalogueItem, ListPriceModel>()
                .ForMember(
                    dest => dest.FlatListPrices,
                    opt =>
                    {
                        opt.PreCondition(src => src.CataloguePrices != null);
                        opt.MapFrom(src => src.CataloguePrices.Where(p => p.CataloguePriceType == CataloguePriceType.Flat));
                    })
                .IncludeBase<CatalogueItem, SolutionDisplayBaseModel>();

            CreateMap<CataloguePrice, PriceViewModel>()
                .ForMember(
                    dest => dest.CurrencyCode,
                    opt => opt.MapFrom(
                        src => CurrencyCodeSigns.Code.ContainsKey(src.CurrencyCode)
                            ? CurrencyCodeSigns.Code[src.CurrencyCode]
                            : null))
                .ForMember(
                    dest => dest.Price,
                    opt =>
                    {
                        opt.PreCondition(src => src.Price != null);
                        opt.MapFrom(src => Math.Round(src.Price.Value, 2));
                    })
                .ForMember(
                    dest => dest.Unit,
                    opt => opt.MapFrom(
                        src =>
                            $"{(src.PricingUnit == null ? string.Empty : src.PricingUnit.Description)} {(src.TimeUnit == null ? string.Empty : src.TimeUnit.Value.Description())}"));

            CreateMap<CataloguePrice, string>()
                .ConstructUsing(
                    src => src.Price == null
                        ? null
                        : $"£{src.Price.Value:F} {(src.PricingUnit != null ? src.PricingUnit.Description : null)}"
                            .Trim());

            CreateMap<CatalogueItemCapability, RowViewModel>()
                .ForMember(
                    dest => dest.Heading,
                    opt => opt.MapFrom(src => src.Capability == null ? null : src.Capability.Name))
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.Capability == null ? null : src.Capability.Description))
                .ForMember(dest => dest.CheckEpicsUrl, opt => opt.MapFrom(src => "capability/" + src.CapabilityId));
        }

        private static List<string> GetEpics(Capability capability, bool supplierDefined) =>
            capability?.Epics == null ?
                new List<string>() :
                capability.Epics.Where(e => e.Active && e.SupplierDefined == supplierDefined).Select(epic => epic.Name).ToList();

        private static IDictionary<string, ListViewModel> GetBrowserBasedItems(ClientApplication clientApplication)
        {
            var result = new Dictionary<string, ListViewModel>();

            if (clientApplication.BrowsersSupported?.Any() == true)
            {
                result.Add(
                    "Supported browser types",
                    new ListViewModel { List = clientApplication.BrowsersSupported.ToArray(), });
            }

            if (clientApplication.MobileResponsive is not null)
            {
                result.Add(
                    "Mobile responsive",
                    new ListViewModel { Text = clientApplication.MobileResponsive.ToYesNo(), });
            }

            if (clientApplication.Plugins is not null)
            {
                result.Add(
                    "Plug-ins or extensions required",
                    new ListViewModel { Text = clientApplication.Plugins.Required.ToYesNo(), });

                if (!string.IsNullOrWhiteSpace(clientApplication.Plugins.AdditionalInformation))
                {
                    result.Add(
                        "Additional information about plug-ins or extensions",
                        new ListViewModel { Text = clientApplication.Plugins.AdditionalInformation, });
                }
            }

            if (!string.IsNullOrWhiteSpace(clientApplication.MinimumConnectionSpeed))
            {
                result.Add(
                    "Minimum connection speed",
                    new ListViewModel { Text = clientApplication.MinimumConnectionSpeed, });
            }

            if (!string.IsNullOrWhiteSpace(clientApplication.MinimumDesktopResolution))
            {
                result.Add(
                    "Screen resolution and aspect ratio",
                    new ListViewModel { Text = clientApplication.MinimumDesktopResolution, });
            }

            if (!string.IsNullOrWhiteSpace(clientApplication.HardwareRequirements))
            {
                result.Add(
                    "Hardware requirements",
                    new ListViewModel { Text = clientApplication.HardwareRequirements });
            }

            if (!string.IsNullOrWhiteSpace(clientApplication.AdditionalInformation))
            {
                result.Add(
                    "Additional information",
                    new ListViewModel { Text = clientApplication.AdditionalInformation });
            }

            return result;
        }

        private static IDictionary<string, ListViewModel> GetNativeDesktopItems(ClientApplication clientApplication)
        {
            var result = new Dictionary<string, ListViewModel>();
            if (!string.IsNullOrWhiteSpace(clientApplication.NativeDesktopOperatingSystemsDescription))
            {
                result.Add(
                    "Description of supported operating systems",
                    new ListViewModel { Text = clientApplication.NativeDesktopOperatingSystemsDescription });
            }

            if (!string.IsNullOrWhiteSpace(clientApplication.NativeDesktopMinimumConnectionSpeed))
            {
                result.Add(
                    "Minimum connection speed",
                    new ListViewModel { Text = clientApplication.NativeDesktopMinimumConnectionSpeed });
            }

            if (!string.IsNullOrWhiteSpace(
                clientApplication.NativeDesktopMemoryAndStorage?.RecommendedResolution))
            {
                result.Add(
                    "Screen resolution and aspect ratio",
                    new ListViewModel
                    {
                        Text = clientApplication.NativeDesktopMemoryAndStorage.RecommendedResolution,
                    });
            }

            if (!string.IsNullOrWhiteSpace(
                clientApplication.NativeDesktopMemoryAndStorage?.MinimumMemoryRequirement))
            {
                result.Add(
                    "Memory size",
                    new ListViewModel
                    {
                        Text = clientApplication.NativeDesktopMemoryAndStorage.MinimumMemoryRequirement,
                    });
            }

            if (!string.IsNullOrWhiteSpace(
                clientApplication.NativeDesktopMemoryAndStorage?.StorageRequirementsDescription))
            {
                result.Add(
                    "Storage space",
                    new ListViewModel
                    {
                        Text = clientApplication.NativeDesktopMemoryAndStorage.StorageRequirementsDescription,
                    });
            }

            if (!string.IsNullOrWhiteSpace(clientApplication.NativeDesktopMemoryAndStorage?.MinimumCpu))
            {
                result.Add(
                    "Processing power",
                    new ListViewModel { Text = clientApplication.NativeDesktopMemoryAndStorage.MinimumCpu });
            }

            if (!string.IsNullOrWhiteSpace(clientApplication.NativeDesktopThirdParty?.ThirdPartyComponents))
            {
                result.Add(
                    "Third-party components",
                    new ListViewModel { Text = clientApplication.NativeDesktopThirdParty.ThirdPartyComponents });
            }

            if (!string.IsNullOrWhiteSpace(clientApplication.NativeDesktopThirdParty?.DeviceCapabilities))
            {
                result.Add(
                    "Device capabilities",
                    new ListViewModel { Text = clientApplication.NativeDesktopThirdParty.DeviceCapabilities });
            }

            if (!string.IsNullOrWhiteSpace(clientApplication.NativeDesktopHardwareRequirements))
            {
                result.Add(
                    "Hardware requirements",
                    new ListViewModel { Text = clientApplication.NativeDesktopHardwareRequirements });
            }

            if (!string.IsNullOrWhiteSpace(clientApplication.NativeDesktopAdditionalInformation))
            {
                result.Add(
                    "Additional information",
                    new ListViewModel { Text = clientApplication.NativeDesktopAdditionalInformation });
            }

            return result;
        }

        private static IDictionary<string, ListViewModel> GetNativeMobileItems(ClientApplication clientApplication)
        {
            var result = new Dictionary<string, ListViewModel>();

            if (clientApplication.MobileOperatingSystems?.OperatingSystems?.Any() == true)
            {
                result.Add(
                    "Supported operating systems",
                    new ListViewModel { List = clientApplication.MobileOperatingSystems.OperatingSystems.ToArray(), });
            }

            if (!string.IsNullOrWhiteSpace(
                clientApplication.MobileOperatingSystems?.OperatingSystemsDescription))
            {
                result.Add(
                    "Description of supported operating systems",
                    new ListViewModel { Text = clientApplication.MobileOperatingSystems.OperatingSystemsDescription });
            }

            if (!string.IsNullOrWhiteSpace(
                clientApplication.MobileConnectionDetails?.MinimumConnectionSpeed))
            {
                result.Add(
                    "Minimum connection speed",
                    new ListViewModel { Text = clientApplication.MobileConnectionDetails.MinimumConnectionSpeed });
            }

            if (clientApplication.MobileConnectionDetails?.ConnectionType?.Any() == true)
            {
                result.Add(
                    "Connection types supported",
                    new ListViewModel { List = clientApplication.MobileConnectionDetails.ConnectionType.ToArray(), });
            }

            if (!string.IsNullOrWhiteSpace(clientApplication.MobileConnectionDetails?.Description))
            {
                result.Add(
                    "Connection requirements",
                    new ListViewModel { Text = clientApplication.MobileConnectionDetails.Description });
            }

            if (!string.IsNullOrWhiteSpace(
                clientApplication.MobileMemoryAndStorage?.MinimumMemoryRequirement))
            {
                result.Add(
                    "Memory size",
                    new ListViewModel { Text = clientApplication.MobileMemoryAndStorage.MinimumMemoryRequirement });
            }

            if (!string.IsNullOrWhiteSpace(clientApplication.MobileMemoryAndStorage?.Description))
            {
                result.Add(
                    "Storage space",
                    new ListViewModel { Text = clientApplication.MobileMemoryAndStorage.Description });
            }

            if (!string.IsNullOrWhiteSpace(clientApplication.MobileThirdParty?.ThirdPartyComponents))
            {
                result.Add(
                    "Third-party components",
                    new ListViewModel { Text = clientApplication.MobileThirdParty.ThirdPartyComponents });
            }

            if (!string.IsNullOrWhiteSpace(clientApplication.MobileThirdParty?.DeviceCapabilities))
            {
                result.Add(
                    "Device capabilities",
                    new ListViewModel { Text = clientApplication.MobileThirdParty.DeviceCapabilities });
            }

            if (!string.IsNullOrWhiteSpace(clientApplication.NativeMobileHardwareRequirements))
            {
                result.Add(
                    "Hardware requirements",
                    new ListViewModel { Text = clientApplication.NativeMobileHardwareRequirements });
            }

            if (!string.IsNullOrWhiteSpace(clientApplication.NativeMobileAdditionalInformation))
            {
                result.Add(
                    "Additional information",
                    new ListViewModel { Text = clientApplication.NativeMobileAdditionalInformation });
            }

            return result;
        }
    }
}