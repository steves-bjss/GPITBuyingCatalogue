﻿using System;
using System.Collections.Generic;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils.RandomData;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Utils.SeedData
{
    public static class CatalogueSolutionSeedData
    {
        public static List<CatalogueItem> GetCatalogueSolutionItems()
        {
            var catalogueItems = new List<CatalogueItem>
            {
                new CatalogueItem
                {
                    Id = new CatalogueItemId(99999, "001"),
                    SupplierId = 99999,
                    CatalogueItemType = CatalogueItemType.Solution,
                    Created = DateTime.UtcNow,
                    Name = "DFOCVC Solution Full",
                    Solution = new Solution
                    {
                        CatalogueItemId = new CatalogueItemId(99999, "001"),
                        AboutUrl = "https://test.com",
                        Summary = "SUMMARY - DFOCVC Solution",
                        Features = @"[""Digital Online Consultation"",""Video Consultation"", ""Fully interopable with all major GP IT solutions"", ""Compliant with all relevant ISO standards""]",
                        Hosting = @"{""PublicCloud"":{""Summary"":""Summary description"",""Link"":""External URL link"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""},""PrivateCloud"":{""Summary"":""Summary description"",""Link"":""External URL link"",""HostingModel"":""Hosting environment description"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""},""HybridHostingType"":{""Summary"":""Summary description"",""Link"":""External URL link"",""HostingModel"":""Hosting environment description"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""},""OnPremise"":{""Summary"":""Summary description"",""Link"":""External URL link"",""HostingModel"":""Hosting environment description"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""}}",
                        ClientApplication = @"{
                            ""ClientApplicationTypes"": [
                                ""browser-based"",
                                ""native-mobile"",
                                ""native-desktop""
                            ],
                            ""BrowsersSupported"": [
                                ""Google Chrome"",
                                ""Chromium"",
                                ""Internet Explorer 11"",
                                ""Internet Explorer 10""
                            ],
                            ""MobileResponsive"": true,
                            ""Plugins"": {
                                ""Required"": false,
                                ""AdditionalInformation"": ""Additional information""
                            },
                            ""MinimumConnectionSpeed"": ""2Mbps"",
                            ""MinimumDesktopResolution"": ""16:9 – 1366 x 768"",
                            ""HardwareRequirements"": ""Something related to Desktop Hardware Requirements"",
                            ""NativeMobileHardwareRequirements"": ""Something related to Mobile requirements"",
                            ""NativeDesktopHardwareRequirements"": ""Something related to Desktop Hardware Requirements"",
                            ""AdditionalInformation"": ""Here is some additional information"",
                            ""MobileFirstDesign"": true,
                            ""NativeMobileFirstDesign"": true,
                            ""MobileOperatingSystems"": {
                                ""OperatingSystems"": [
                                    ""Apple IOS"",
                                    ""Android"",
                                    ""Other""
                                ],
                                ""OperatingSystemsDescription"": ""•\tiOS v 10.3.3.3 and above\r\n\r\n•\tAndroid v 6 and above\r\n\r\n•\tWindows 10 (Build 14393)""
                            },
                            ""MobileConnectionDetails"": {
                                ""MinimumConnectionSpeed"": ""2Mbps"",
                                ""ConnectionType"": [
                                    ""GPRS"",
                                    ""3G"",
                                    ""LTE"",
                                    ""4G"",
                                    ""5G"",
                                    ""Wifi""
                                ],
                                ""Description"": ""Mobile connection required""
                            },
                            ""MobileMemoryAndStorage"": {
                                ""MinimumMemoryRequirement"": ""2GB"",
                                ""Description"": ""All compliant devices must have a minimum 16GB storage.""
                            },
                            ""MobileThirdParty"": {
                                ""ThirdPartyComponents"": ""Some third party stuff may be supported"",
                                ""DeviceCapabilities"": ""Must have app""
                            },
                            ""NativeMobileAdditionalInformation"": ""Tested on multiple operating systems"",
                            ""NativeDesktopOperatingSystemsDescription"": ""Microsoft Windows 7 (x86 x64)\r\n\r\nMicrosoft Windows 8.1 (x86 x64)\r\n\r\nMicrosoft Windows 10 (x86 x64)"",
                            ""NativeDesktopMinimumConnectionSpeed"": ""2Mbps"",
                            ""NativeDesktopThirdParty"": {
                                ""ThirdPartyComponents"": "".NET framework 4."",
                                ""DeviceCapabilities"": ""The application requires connectivity to the Internet""
                            },
                            ""NativeDesktopMemoryAndStorage"": {
                                ""MinimumMemoryRequirement"": ""4GB"",
                                ""StorageRequirementsDescription"": ""10GB free disk space."",
                                ""MinimumCpu"": ""Intel Core i3 equivalent or higher."",
                                ""RecommendedResolution"": ""16:9 – 1366 x 768""
                            },
                            ""NativeDesktopAdditionalInformation"": ""The minimum connection speed is dependent on the number of clients that need to be supported.""
                        }",
                        LastUpdated = DateTime.UtcNow,
                        FullDescription = "FULL DESCRIPTION – Digital First, Online Consultation and Video Consultation Solution.",
                        ImplementationDetail = "Some implementation detail",
                        MarketingContacts = new List<MarketingContact>
                        {
                            new()
                            {
                                FirstName = "Bob",
                                LastName = "Smith",
                                Email = "test@test.com",
                                Department = "Fruit controller",
                                LastUpdated = DateTime.UtcNow,
                            },
                        },
                    },
                    CatalogueItemCapabilities = new List<CatalogueItemCapability>
                        {
                            new() { CapabilityId = 43, LastUpdated = DateTime.UtcNow, StatusId = 1 },
                        },
                    CatalogueItemEpics = new List<CatalogueItemEpic>
                        {
                            new() { CapabilityId = 43, EpicId = "E00001", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00002", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00003", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00004", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00005", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00006", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00007", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00008", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00009", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00010", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00011", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00012", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00013", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00014", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00015", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00016", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00017", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00018", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00019", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00020", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00021", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00022", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00023", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00024", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00025", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00026", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00027", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00028", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00029", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00030", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00031", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00032", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00033", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00034", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00035", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00037", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00038", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00056", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00057", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00058", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00075", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00076", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00077", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00078", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00079", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00080", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00081", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00082", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00083", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00084", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00085", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00086", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00089", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00090", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00091", StatusId = 1 },
                            new() { CapabilityId = 43, EpicId = "E00099", StatusId = 1 },
                        },
                    PublishedStatus = PublicationStatus.Published,
                },
                new CatalogueItem
                {
                    Id = new CatalogueItemId(99999, "002"),
                    SupplierId = 99999,
                    CatalogueItemType = CatalogueItemType.Solution,
                    Created = DateTime.UtcNow,
                    Name = "DFOCVC Solution Empty",

                    PublishedStatus = PublicationStatus.Draft,
                    Solution = new Solution
                    {
                        CatalogueItemId = new CatalogueItemId(99999, "002"),
                        LastUpdated = DateTime.UtcNow,
                    },
                },
                new CatalogueItem
                {
                    Id = new CatalogueItemId(99999, "003"),
                    SupplierId = 99999,
                    CatalogueItemType = CatalogueItemType.Solution,
                    Created = DateTime.UtcNow,
                    Name = "GPIT Solution Full",
                    Solution = new Solution
                    {
                        CatalogueItemId = new CatalogueItemId(99999, "003"),
                        AboutUrl = "https://test.com",
                        Summary = "SUMMARY - GPIT Solution",
                        Features = @"[""Digital Online Consultation"",""Video Consultation"", ""Fully interopable with all major GP IT solutions"", ""Compliant with all relevant ISO standards""]",
                        Hosting = @"{""PublicCloud"":{""Summary"":""Summary description"",""Link"":""External URL link"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""},""PrivateCloud"":{""Summary"":""Summary description"",""Link"":""External URL link"",""HostingModel"":""Hosting environment description"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""},""HybridHostingType"":{""Summary"":""Summary description"",""Link"":""External URL link"",""HostingModel"":""Hosting environment description"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""},""OnPremise"":{""Summary"":""Summary description"",""Link"":""External URL link"",""HostingModel"":""Hosting environment description"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""}}",
                        ClientApplication = @"{
                            ""ClientApplicationTypes"": [
                                ""browser-based"",
                                ""native-mobile"",
                                ""native-desktop""
                            ],
                            ""BrowsersSupported"": [
                                ""Google Chrome"",
                                ""Chromium"",
                                ""Internet Explorer 11"",
                                ""Internet Explorer 10""
                            ],
                            ""MobileResponsive"": true,
                            ""Plugins"": {
                                ""Required"": false,
                                ""AdditionalInformation"": ""Additional information""
                            },
                            ""MinimumConnectionSpeed"": ""2Mbps"",
                            ""MinimumDesktopResolution"": ""16:9 – 1366 x 768"",
                            ""HardwareRequirements"": ""Something related to Desktop Hardware Requirements"",
                            ""NativeMobileHardwareRequirements"": ""Something related to Mobile requirements"",
                            ""NativeDesktopHardwareRequirements"": ""Something related to Desktop Hardware Requirements"",
                            ""AdditionalInformation"": ""Here is some additional information"",
                            ""MobileFirstDesign"": true,
                            ""NativeMobileFirstDesign"": true,
                            ""MobileOperatingSystems"": {
                                ""OperatingSystems"": [
                                    ""Apple IOS"",
                                    ""Android"",
                                    ""Other""
                                ],
                                ""OperatingSystemsDescription"": ""•\tiOS v 10.3.3.3 and above\r\n\r\n•\tAndroid v 6 and above\r\n\r\n•\tWindows 10 (Build 14393)""
                            },
                            ""MobileConnectionDetails"": {
                                ""MinimumConnectionSpeed"": ""2Mbps"",
                                ""ConnectionType"": [
                                    ""GPRS"",
                                    ""3G"",
                                    ""LTE"",
                                    ""4G"",
                                    ""5G"",
                                    ""Wifi""
                                ],
                                ""Description"": ""Mobile connection required""
                            },
                            ""MobileMemoryAndStorage"": {
                                ""MinimumMemoryRequirement"": ""2GB"",
                                ""Description"": ""All compliant devices must have a minimum 16GB storage.""
                            },
                            ""MobileThirdParty"": {
                                ""ThirdPartyComponents"": ""Some third party stuff may be supported"",
                                ""DeviceCapabilities"": ""Must have app""
                            },
                            ""NativeMobileAdditionalInformation"": ""Tested on multiple operating systems"",
                            ""NativeDesktopOperatingSystemsDescription"": ""Microsoft Windows 7 (x86 x64)\r\n\r\nMicrosoft Windows 8.1 (x86 x64)\r\n\r\nMicrosoft Windows 10 (x86 x64)"",
                            ""NativeDesktopMinimumConnectionSpeed"": ""2Mbps"",
                            ""NativeDesktopThirdParty"": {
                                ""ThirdPartyComponents"": "".NET framework 4."",
                                ""DeviceCapabilities"": ""The application requires connectivity to the Internet""
                            },
                            ""NativeDesktopMemoryAndStorage"": {
                                ""MinimumMemoryRequirement"": ""4GB"",
                                ""StorageRequirementsDescription"": ""10GB free disk space."",
                                ""MinimumCpu"": ""Intel Core i3 equivalent or higher."",
                                ""RecommendedResolution"": ""16:9 – 1366 x 768""
                            },
                            ""NativeDesktopAdditionalInformation"": ""The minimum connection speed is dependent on the number of clients that need to be supported.""
                        }",
                        LastUpdated = DateTime.UtcNow,
                        FullDescription = "FULL DESCRIPTION – GP IT Futures Solution.",
                        ImplementationDetail = "Some implementation detail",
                        MarketingContacts = new List<MarketingContact>
                        {
                            new()
                            {
                                FirstName = "Geoff",
                                LastName = "Jones",
                                Email = "test@test.com",
                                Department = "Fruit wrangler",
                                LastUpdated = DateTime.UtcNow,
                            },
                        },
                    },
                    CatalogueItemCapabilities = new List<CatalogueItemCapability>
                        {
                            new() { CapabilityId = 1, LastUpdated = DateTime.UtcNow, StatusId = 1 },
                        },
                    CatalogueItemEpics = new List<CatalogueItemEpic>
                        {
                            new() { CapabilityId = 1, EpicId = "C1E1", StatusId = 1 },
                            new() { CapabilityId = 1, EpicId = "C1E2", StatusId = 2 },
                        },
                    PublishedStatus = PublicationStatus.Published,
                },
                new CatalogueItem
                {
                    Id = new CatalogueItemId(99999, "004"),
                    SupplierId = 99999,
                    CatalogueItemType = CatalogueItemType.Solution,
                    Created = DateTime.UtcNow,
                    Name = "GPIT Solution Full 2",
                    Solution = new Solution
                    {
                        CatalogueItemId = new CatalogueItemId(99999, "004"),
                        AboutUrl = "https://test.com",
                        Summary = "SUMMARY - GPIT Solution 2",
                        Features = @"[""Digital Online Consultation"",""Video Consultation"", ""Fully interopable with all major GP IT solutions"", ""Compliant with all relevant ISO standards""]",
                        Hosting = @"{""PublicCloud"":{""Summary"":""Summary description"",""Link"":""External URL link"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""},""PrivateCloud"":{""Summary"":""Summary description"",""Link"":""External URL link"",""HostingModel"":""Hosting environment description"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""},""HybridHostingType"":{""Summary"":""Summary description"",""Link"":""External URL link"",""HostingModel"":""Hosting environment description"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""},""OnPremise"":{""Summary"":""Summary description"",""Link"":""External URL link"",""HostingModel"":""Hosting environment description"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""}}",
                        ClientApplication = @"{
                            ""ClientApplicationTypes"": [
                                ""browser-based"",
                                ""native-mobile"",
                                ""native-desktop""
                            ],
                            ""BrowsersSupported"": [
                                ""Google Chrome"",
                                ""Chromium"",
                                ""Internet Explorer 11"",
                                ""Internet Explorer 10""
                            ],
                            ""MobileResponsive"": true,
                            ""Plugins"": {
                                ""Required"": false,
                                ""AdditionalInformation"": ""Additional information""
                            },
                            ""MinimumConnectionSpeed"": ""2Mbps"",
                            ""MinimumDesktopResolution"": ""16:9 – 1366 x 768"",
                            ""HardwareRequirements"": ""Something related to Desktop Hardware Requirements"",
                            ""NativeMobileHardwareRequirements"": ""Something related to Mobile requirements"",
                            ""NativeDesktopHardwareRequirements"": ""Something related to Desktop Hardware Requirements"",
                            ""AdditionalInformation"": ""Here is some additional information"",
                            ""MobileFirstDesign"": true,
                            ""NativeMobileFirstDesign"": true,
                            ""MobileOperatingSystems"": {
                                ""OperatingSystems"": [
                                    ""Apple IOS"",
                                    ""Android"",
                                    ""Other""
                                ],
                                ""OperatingSystemsDescription"": ""•\tiOS v 10.3.3.3 and above\r\n\r\n•\tAndroid v 6 and above\r\n\r\n•\tWindows 10 (Build 14393)""
                            },
                            ""MobileConnectionDetails"": {
                                ""MinimumConnectionSpeed"": ""2Mbps"",
                                ""ConnectionType"": [
                                    ""GPRS"",
                                    ""3G"",
                                    ""LTE"",
                                    ""4G"",
                                    ""5G"",
                                    ""Wifi""
                                ],
                                ""Description"": ""Mobile connection required""
                            },
                            ""MobileMemoryAndStorage"": {
                                ""MinimumMemoryRequirement"": ""2GB"",
                                ""Description"": ""All compliant devices must have a minimum 16GB storage.""
                            },
                            ""MobileThirdParty"": {
                                ""ThirdPartyComponents"": ""Some third party stuff may be supported"",
                                ""DeviceCapabilities"": ""Must have app""
                            },
                            ""NativeMobileAdditionalInformation"": ""Tested on multiple operating systems"",
                            ""NativeDesktopOperatingSystemsDescription"": ""Microsoft Windows 7 (x86 x64)\r\n\r\nMicrosoft Windows 8.1 (x86 x64)\r\n\r\nMicrosoft Windows 10 (x86 x64)"",
                            ""NativeDesktopMinimumConnectionSpeed"": ""2Mbps"",
                            ""NativeDesktopThirdParty"": {
                                ""ThirdPartyComponents"": "".NET framework 4."",
                                ""DeviceCapabilities"": ""The application requires connectivity to the Internet""
                            },
                            ""NativeDesktopMemoryAndStorage"": {
                                ""MinimumMemoryRequirement"": ""4GB"",
                                ""StorageRequirementsDescription"": ""10GB free disk space."",
                                ""MinimumCpu"": ""Intel Core i3 equivalent or higher."",
                                ""RecommendedResolution"": ""16:9 – 1366 x 768""
                            },
                            ""NativeDesktopAdditionalInformation"": ""The minimum connection speed is dependent on the number of clients that need to be supported.""
                        }",
                        LastUpdated = DateTime.UtcNow,
                        FullDescription = "FULL DESCRIPTION – GP IT Futures Solution 2.",
                        ImplementationDetail = "Some implementation detail",
                        MarketingContacts = new List<MarketingContact>
                        {
                            new()
                            {
                                FirstName = "Geoff",
                                LastName = "Jones",
                                Email = "test@test.com",
                                Department = "Fruit wrangler",
                                LastUpdated = DateTime.UtcNow,
                            },
                        },
                    },
                    CatalogueItemCapabilities = new List<CatalogueItemCapability>
                        {
                            new() { CapabilityId = 2, LastUpdated = DateTime.UtcNow, StatusId = 1 },
                        },
                    CatalogueItemEpics = new List<CatalogueItemEpic>
                        {
                            new() { CapabilityId = 2, EpicId = "C2E1", StatusId = 1 },
                            new() { CapabilityId = 2, EpicId = "C2E2", StatusId = 2 },
                        },
                    PublishedStatus = PublicationStatus.Published,
                },
                new CatalogueItem
                {
                    Id = new CatalogueItemId(99999, "005"),
                    PublishedStatus = PublicationStatus.Draft,
                    CatalogueItemType = CatalogueItemType.Solution,
                    SupplierId = 99999,
                    Name = "Editable Solution",
                    Created = DateTime.UtcNow,
                    Solution = new Solution
                    {
                        CatalogueItemId = new CatalogueItemId(99999, "005"),
                        LastUpdatedBy = UserSeedData.BobId,
                        LastUpdated = DateTime.UtcNow,
                    },
                },
                new CatalogueItem
                {
                    Id = new CatalogueItemId(99997, "001"),
                    SupplierId = 99997,
                    CatalogueItemType = CatalogueItemType.Solution,
                    Created = DateTime.UtcNow,
                    Name = "E2E With No Contact Solution",
                    Solution = new Solution
                    {
                        CatalogueItemId = new CatalogueItemId(99999, "006"),
                        AboutUrl = "https://test.com",
                        Summary = "SUMMARY - DFOCVC Solution",
                        Features = @"[""Digital Online Consultation"",""Video Consultation"", ""Fully interopable with all major GP IT solutions"", ""Compliant with all relevant ISO standards""]",
                        Hosting = @"{""PublicCloud"":{""Summary"":""Summary description"",""Link"":""External URL link"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""},""PrivateCloud"":{""Summary"":""Summary description"",""Link"":""External URL link"",""HostingModel"":""Hosting environment description"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""},""HybridHostingType"":{""Summary"":""Summary description"",""Link"":""External URL link"",""HostingModel"":""Hosting environment description"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""},""OnPremise"":{""Summary"":""Summary description"",""Link"":""External URL link"",""HostingModel"":""Hosting environment description"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""}}",
                        ClientApplication = @"{
                            ""ClientApplicationTypes"": [
                                ""browser-based"",
                                ""native-mobile"",
                                ""native-desktop""
                            ],
                            ""BrowsersSupported"": [
                                ""Google Chrome"",
                                ""Chromium"",
                                ""Internet Explorer 11"",
                                ""Internet Explorer 10""
                            ],
                            ""MobileResponsive"": true,
                            ""Plugins"": {
                                ""Required"": false,
                                ""AdditionalInformation"": ""Additional information""
                            },
                            ""MinimumConnectionSpeed"": ""2Mbps"",
                            ""MinimumDesktopResolution"": ""16:9 – 1366 x 768"",
                            ""HardwareRequirements"": ""Something related to Desktop Hardware Requirements"",
                            ""NativeMobileHardwareRequirements"": ""Something related to Mobile requirements"",
                            ""NativeDesktopHardwareRequirements"": ""Something related to Desktop Hardware Requirements"",
                            ""AdditionalInformation"": ""Here is some additional information"",
                            ""MobileFirstDesign"": true,
                            ""NativeMobileFirstDesign"": true,
                            ""MobileOperatingSystems"": {
                                ""OperatingSystems"": [
                                    ""Apple IOS"",
                                    ""Android"",
                                    ""Other""
                                ],
                                ""OperatingSystemsDescription"": ""•\tiOS v 10.3.3.3 and above\r\n\r\n•\tAndroid v 6 and above\r\n\r\n•\tWindows 10 (Build 14393)""
                            },
                            ""MobileConnectionDetails"": {
                                ""MinimumConnectionSpeed"": ""2Mbps"",
                                ""ConnectionType"": [
                                    ""GPRS"",
                                    ""3G"",
                                    ""LTE"",
                                    ""4G"",
                                    ""5G"",
                                    ""Wifi""
                                ],
                                ""Description"": ""Mobile connection required""
                            },
                            ""MobileMemoryAndStorage"": {
                                ""MinimumMemoryRequirement"": ""2GB"",
                                ""Description"": ""All compliant devices must have a minimum 16GB storage.""
                            },
                            ""MobileThirdParty"": {
                                ""ThirdPartyComponents"": ""Some third party stuff may be supported"",
                                ""DeviceCapabilities"": ""Must have app""
                            },
                            ""NativeMobileAdditionalInformation"": ""Tested on multiple operating systems"",
                            ""NativeDesktopOperatingSystemsDescription"": ""Microsoft Windows 7 (x86 x64)\r\n\r\nMicrosoft Windows 8.1 (x86 x64)\r\n\r\nMicrosoft Windows 10 (x86 x64)"",
                            ""NativeDesktopMinimumConnectionSpeed"": ""2Mbps"",
                            ""NativeDesktopThirdParty"": {
                                ""ThirdPartyComponents"": "".NET framework 4."",
                                ""DeviceCapabilities"": ""The application requires connectivity to the Internet""
                            },
                            ""NativeDesktopMemoryAndStorage"": {
                                ""MinimumMemoryRequirement"": ""4GB"",
                                ""StorageRequirementsDescription"": ""10GB free disk space."",
                                ""MinimumCpu"": ""Intel Core i3 equivalent or higher."",
                                ""RecommendedResolution"": ""16:9 – 1366 x 768""
                            },
                            ""NativeDesktopAdditionalInformation"": ""The minimum connection speed is dependent on the number of clients that need to be supported.""
                        }",
                        LastUpdated = DateTime.UtcNow,
                        FullDescription = "FULL DESCRIPTION – Digital First, Online Consultation and Video Consultation Solution.",
                        ImplementationDetail = "Some implementation detail",
                        MarketingContacts = new List<MarketingContact>
                        {
                            new()
                            {
                                FirstName = "Bob",
                                LastName = "Smith",
                                Email = "test@test.com",
                                Department = "Fruit controller",
                                LastUpdated = DateTime.UtcNow,
                            },
                        },
                    },
                    CatalogueItemCapabilities = new List<CatalogueItemCapability>
                    {
                        new() { CapabilityId = 46, LastUpdated = DateTime.UtcNow, StatusId = 1 },
                    },
                    CatalogueItemEpics = new List<CatalogueItemEpic>
                    {
                        new() { CapabilityId = 46, EpicId = "C46E1", LastUpdated = DateTime.UtcNow, StatusId = 1 },
                    },
                    PublishedStatus = PublicationStatus.Published,
                    CataloguePrices = new List<CataloguePrice>
                    {
                        new()
                        {
                            CataloguePriceId = 5,
                            CatalogueItemId = new CatalogueItemId(99997, "001"),
                            ProvisioningType = ProvisioningType.Patient,
                            CataloguePriceType = CataloguePriceType.Flat,
                            PricingUnit = new PricingUnit
                            {
                                Id = 5,
                                Name = "Test Pricing Patient No Contact",
                                TierName = "Test Tier",
                                Description = "per test patient",
                            },
                            TimeUnit = TimeUnit.PerYear,
                            CurrencyCode = "GBP",
                            Price = 999.9999M,
                            LastUpdated = DateTime.UtcNow,
                        },
                    },
                },
                new CatalogueItem
                {
                    Id = new CatalogueItemId(99998, "001"),
                    SupplierId = 99998,
                    CatalogueItemType = CatalogueItemType.Solution,
                    Created = DateTime.UtcNow,
                    Name = "E2E With Contact Multiple Prices",
                    Solution = new Solution
                    {
                        CatalogueItemId = new CatalogueItemId(99998, "001"),
                        AboutUrl = "https://test.com",
                        Summary = "SUMMARY - E2E With Contact Multiple Prices",
                        Features = @"[""Digital Online Consultation"",""Video Consultation"", ""Fully interopable with all major GP IT solutions"", ""Compliant with all relevant ISO standards""]",
                        Hosting = @"{""PublicCloud"":{""Summary"":""Summary description"",""Link"":""External URL link"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""},""PrivateCloud"":{""Summary"":""Summary description"",""Link"":""External URL link"",""HostingModel"":""Hosting environment description"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""},""HybridHostingType"":{""Summary"":""Summary description"",""Link"":""External URL link"",""HostingModel"":""Hosting environment description"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""},""OnPremise"":{""Summary"":""Summary description"",""Link"":""External URL link"",""HostingModel"":""Hosting environment description"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""}}",
                        ClientApplication = @"{
                            ""ClientApplicationTypes"": [
                                ""browser-based"",
                                ""native-mobile"",
                                ""native-desktop""
                            ],
                            ""BrowsersSupported"": [
                                ""Google Chrome"",
                                ""Chromium"",
                                ""Internet Explorer 11"",
                                ""Internet Explorer 10""
                            ],
                            ""MobileResponsive"": true,
                            ""Plugins"": {
                                ""Required"": false,
                                ""AdditionalInformation"": ""Additional information""
                            },
                            ""MinimumConnectionSpeed"": ""2Mbps"",
                            ""MinimumDesktopResolution"": ""16:9 – 1366 x 768"",
                            ""HardwareRequirements"": ""Something related to Desktop Hardware Requirements"",
                            ""NativeMobileHardwareRequirements"": ""Something related to Mobile requirements"",
                            ""NativeDesktopHardwareRequirements"": ""Something related to Desktop Hardware Requirements"",
                            ""AdditionalInformation"": ""Here is some additional information"",
                            ""MobileFirstDesign"": true,
                            ""NativeMobileFirstDesign"": true,
                            ""MobileOperatingSystems"": {
                                ""OperatingSystems"": [
                                    ""Apple IOS"",
                                    ""Android"",
                                    ""Other""
                                ],
                                ""OperatingSystemsDescription"": ""•\tiOS v 10.3.3.3 and above\r\n\r\n•\tAndroid v 6 and above\r\n\r\n•\tWindows 10 (Build 14393)""
                            },
                            ""MobileConnectionDetails"": {
                                ""MinimumConnectionSpeed"": ""2Mbps"",
                                ""ConnectionType"": [
                                    ""GPRS"",
                                    ""3G"",
                                    ""LTE"",
                                    ""4G"",
                                    ""5G"",
                                    ""Wifi""
                                ],
                                ""Description"": ""Mobile connection required""
                            },
                            ""MobileMemoryAndStorage"": {
                                ""MinimumMemoryRequirement"": ""2GB"",
                                ""Description"": ""All compliant devices must have a minimum 16GB storage.""
                            },
                            ""MobileThirdParty"": {
                                ""ThirdPartyComponents"": ""Some third party stuff may be supported"",
                                ""DeviceCapabilities"": ""Must have app""
                            },
                            ""NativeMobileAdditionalInformation"": ""Tested on multiple operating systems"",
                            ""NativeDesktopOperatingSystemsDescription"": ""Microsoft Windows 7 (x86 x64)\r\n\r\nMicrosoft Windows 8.1 (x86 x64)\r\n\r\nMicrosoft Windows 10 (x86 x64)"",
                            ""NativeDesktopMinimumConnectionSpeed"": ""2Mbps"",
                            ""NativeDesktopThirdParty"": {
                                ""ThirdPartyComponents"": "".NET framework 4."",
                                ""DeviceCapabilities"": ""The application requires connectivity to the Internet""
                            },
                            ""NativeDesktopMemoryAndStorage"": {
                                ""MinimumMemoryRequirement"": ""4GB"",
                                ""StorageRequirementsDescription"": ""10GB free disk space."",
                                ""MinimumCpu"": ""Intel Core i3 equivalent or higher."",
                                ""RecommendedResolution"": ""16:9 – 1366 x 768""
                            },
                            ""NativeDesktopAdditionalInformation"": ""The minimum connection speed is dependent on the number of clients that need to be supported.""
                        }",
                        LastUpdated = DateTime.UtcNow,
                        FullDescription = "FULL DESCRIPTION – Digital First, Online Consultation and Video Consultation Solution.",
                        ImplementationDetail = "Some implementation detail",
                        MarketingContacts = new List<MarketingContact>
                        {
                            new()
                            {
                                FirstName = "Bob",
                                LastName = "Smith",
                                Email = "test@test.com",
                                Department = "Fruit controller",
                                LastUpdated = DateTime.UtcNow,
                            },
                        },
                    },
                    CatalogueItemCapabilities = new List<CatalogueItemCapability>
                    {
                        new() { CapabilityId = 5, LastUpdated = DateTime.UtcNow, StatusId = 1 },
                        new() { CapabilityId = 11, LastUpdated = DateTime.UtcNow, StatusId = 1 },
                        new() { CapabilityId = 12, LastUpdated = DateTime.UtcNow, StatusId = 1 },
                        new() { CapabilityId = 13, LastUpdated = DateTime.UtcNow, StatusId = 1 },
                        new() { CapabilityId = 14, LastUpdated = DateTime.UtcNow, StatusId = 1 },
                        new() { CapabilityId = 15, LastUpdated = DateTime.UtcNow, StatusId = 1 },
                        new() { CapabilityId = 46, LastUpdated = DateTime.UtcNow, StatusId = 1 },
                    },
                    CatalogueItemEpics = new List<CatalogueItemEpic>
                    {
                        new() { CapabilityId = 46, EpicId = "C46E2", LastUpdated = DateTime.UtcNow, StatusId = 1 },
                    },
                    PublishedStatus = PublicationStatus.Published,
                    CataloguePrices = new List<CataloguePrice>
                    {
                        new()
                        {
                            CataloguePriceId = 1,
                            CatalogueItemId = new CatalogueItemId(99998, "001"),
                            ProvisioningType = ProvisioningType.Patient,
                            CataloguePriceType = CataloguePriceType.Flat,
                            PricingUnit = new PricingUnit
                            {
                                Id = 1,
                                Name = "Test Pricing Patient",
                                TierName = "Test Tier",
                                Description = "per test patient",
                            },
                            TimeUnit = TimeUnit.PerYear,
                            CurrencyCode = "GBP",
                            Price = 999.9999M,
                            LastUpdated = DateTime.UtcNow,
                        },
                        new()
                        {
                            CataloguePriceId = 2,
                            CatalogueItemId = new CatalogueItemId(99998, "001"),
                            ProvisioningType = ProvisioningType.OnDemand,
                            CataloguePriceType = CataloguePriceType.Flat,
                            PricingUnit = new PricingUnit
                            {
                                Id = 2,
                                Name = "Test Pricing On Demand",
                                TierName = "Test Tier",
                                Description = "per test on demand",
                            },
                            TimeUnit = TimeUnit.PerYear,
                            CurrencyCode = "GBP",
                            Price = 999.9999M,
                            LastUpdated = DateTime.UtcNow,
                        },
                        new()
                        {
                            CataloguePriceId = 3,
                            CatalogueItemId = new CatalogueItemId(99998, "001"),
                            ProvisioningType = ProvisioningType.Declarative,
                            CataloguePriceType = CataloguePriceType.Flat,
                            PricingUnit = new PricingUnit
                            {
                                Id = 3,
                                Name = "Test Pricing Declarative",
                                TierName = "Test Tier",
                                Description = "per test declarative",
                            },
                            TimeUnit = TimeUnit.PerYear,
                            CurrencyCode = "GBP",
                            Price = 999.9999M,
                            LastUpdated = DateTime.UtcNow,
                        },
                    },
                },
                new CatalogueItem
                {
                    Id = new CatalogueItemId(99998, "002"),
                    SupplierId = 99998,
                    CatalogueItemType = CatalogueItemType.Solution,
                    Created = DateTime.UtcNow,
                    Name = "E2E With Contact With Single Price",

                    Solution = new Solution
                    {
                        CatalogueItemId = new CatalogueItemId(99999, "008"),
                        AboutUrl = "https://test.com",
                        Summary = "SUMMARY - E2E With Contact With Single Price",
                        Features = @"[""Digital Online Consultation"",""Video Consultation"", ""Fully interopable with all major GP IT solutions"", ""Compliant with all relevant ISO standards""]",
                        Hosting = @"{""PublicCloud"":{""Summary"":""Summary description"",""Link"":""External URL link"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""},""PrivateCloud"":{""Summary"":""Summary description"",""Link"":""External URL link"",""HostingModel"":""Hosting environment description"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""},""HybridHostingType"":{""Summary"":""Summary description"",""Link"":""External URL link"",""HostingModel"":""Hosting environment description"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""},""OnPremise"":{""Summary"":""Summary description"",""Link"":""External URL link"",""HostingModel"":""Hosting environment description"",""RequiresHSCN"":""Link to HSCN or N3 network required to access service""}}",
                        ClientApplication = @"{
                            ""ClientApplicationTypes"": [
                                ""browser-based"",
                                ""native-mobile"",
                                ""native-desktop""
                            ],
                            ""BrowsersSupported"": [
                                ""Google Chrome"",
                                ""Chromium"",
                                ""Internet Explorer 11"",
                                ""Internet Explorer 10""
                            ],
                            ""MobileResponsive"": true,
                            ""Plugins"": {
                                ""Required"": false,
                                ""AdditionalInformation"": ""Additional information""
                            },
                            ""MinimumConnectionSpeed"": ""2Mbps"",
                            ""MinimumDesktopResolution"": ""16:9 – 1366 x 768"",
                            ""HardwareRequirements"": ""Something related to Desktop Hardware Requirements"",
                            ""NativeMobileHardwareRequirements"": ""Something related to Mobile requirements"",
                            ""NativeDesktopHardwareRequirements"": ""Something related to Desktop Hardware Requirements"",
                            ""AdditionalInformation"": ""Here is some additional information"",
                            ""MobileFirstDesign"": true,
                            ""NativeMobileFirstDesign"": true,
                            ""MobileOperatingSystems"": {
                                ""OperatingSystems"": [
                                    ""Apple IOS"",
                                    ""Android"",
                                    ""Other""
                                ],
                                ""OperatingSystemsDescription"": ""•\tiOS v 10.3.3.3 and above\r\n\r\n•\tAndroid v 6 and above\r\n\r\n•\tWindows 10 (Build 14393)""
                            },
                            ""MobileConnectionDetails"": {
                                ""MinimumConnectionSpeed"": ""2Mbps"",
                                ""ConnectionType"": [
                                    ""GPRS"",
                                    ""3G"",
                                    ""LTE"",
                                    ""4G"",
                                    ""5G"",
                                    ""Wifi""
                                ],
                                ""Description"": ""Mobile connection required""
                            },
                            ""MobileMemoryAndStorage"": {
                                ""MinimumMemoryRequirement"": ""2GB"",
                                ""Description"": ""All compliant devices must have a minimum 16GB storage.""
                            },
                            ""MobileThirdParty"": {
                                ""ThirdPartyComponents"": ""Some third party stuff may be supported"",
                                ""DeviceCapabilities"": ""Must have app""
                            },
                            ""NativeMobileAdditionalInformation"": ""Tested on multiple operating systems"",
                            ""NativeDesktopOperatingSystemsDescription"": ""Microsoft Windows 7 (x86 x64)\r\n\r\nMicrosoft Windows 8.1 (x86 x64)\r\n\r\nMicrosoft Windows 10 (x86 x64)"",
                            ""NativeDesktopMinimumConnectionSpeed"": ""2Mbps"",
                            ""NativeDesktopThirdParty"": {
                                ""ThirdPartyComponents"": "".NET framework 4."",
                                ""DeviceCapabilities"": ""The application requires connectivity to the Internet""
                            },
                            ""NativeDesktopMemoryAndStorage"": {
                                ""MinimumMemoryRequirement"": ""4GB"",
                                ""StorageRequirementsDescription"": ""10GB free disk space."",
                                ""MinimumCpu"": ""Intel Core i3 equivalent or higher."",
                                ""RecommendedResolution"": ""16:9 – 1366 x 768""
                            },
                            ""NativeDesktopAdditionalInformation"": ""The minimum connection speed is dependent on the number of clients that need to be supported.""
                        }",
                        LastUpdated = DateTime.UtcNow,
                        FullDescription = "FULL DESCRIPTION – Digital First, Online Consultation and Video Consultation Solution.",
                        ImplementationDetail = "Some implementation detail",
                        MarketingContacts = new List<MarketingContact>
                        {
                            new()
                            {
                                FirstName = "Bob",
                                LastName = "Smith",
                                Email = "test@test.com",
                                Department = "Fruit controller",
                                LastUpdated = DateTime.UtcNow,
                            },
                        },
                    },
                    CatalogueItemCapabilities = new List<CatalogueItemCapability>
                    {
                        new() { CapabilityId = 47, LastUpdated = DateTime.UtcNow, StatusId = 1 },
                    },
                    CatalogueItemEpics = new List<CatalogueItemEpic>
                    {
                        new() { CapabilityId = 47, EpicId = "C47E1", LastUpdated = DateTime.UtcNow, StatusId = 1 },
                    },
                    PublishedStatus = PublicationStatus.Published,
                    CataloguePrices = new List<CataloguePrice>
                    {
                        new()
                        {
                            CataloguePriceId = 4,
                            CatalogueItemId = new CatalogueItemId(99998, "002"),
                            ProvisioningType = ProvisioningType.Declarative,
                            CataloguePriceType = CataloguePriceType.Flat,
                            PricingUnit = new PricingUnit
                            {
                                Id = 4,
                                Name = "Test Pricing",
                                TierName = "Test Tier",
                                Description = "Per Test",
                            },
                            TimeUnit = TimeUnit.PerYear,
                            CurrencyCode = "GBP",
                            Price = 999.9999M,
                            LastUpdated = DateTime.UtcNow,
                        },
                    },
                },
            };

            catalogueItems.AddRange(GetGeneratedCatalogueSolutionItems());

            return catalogueItems;
        }

        private static List<CatalogueItem> GetGeneratedCatalogueSolutionItems(int count = 100)
        {
            int defaultStart = 400;

            var catalogueItems = new List<CatalogueItem>();

            for (int i = defaultStart; i < (defaultStart + count); i++)
            {
                catalogueItems.Add(GenerateCatalogueSolution.Generate(new EntityFramework.Ordering.Models.CatalogueItemId(99998, i.ToString())));
            }

            return catalogueItems;
        }
    }
}
