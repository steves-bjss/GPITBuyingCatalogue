using System.Linq;
using AutoMapper;
using FluentAssertions;
using Newtonsoft.Json;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.BuyingCatalogue;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;
using NHSD.GPIT.BuyingCatalogue.Test.Framework;
using NHSD.GPIT.BuyingCatalogue.Test.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Marketing.Models.ClientApplicationType;
using NHSD.GPIT.BuyingCatalogue.WebApp.MappingProfiles;
using NUnit.Framework;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.MappingProfiles
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    internal static class ClientApplicationTypeProfileTests
    {
        [Test]
        public static void Mappings_Configuration_Valid()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ClientApplicationTypeProfile>();
                cfg.AddProfile<OrganisationProfile>();
            });

            mapperConfiguration.AssertConfigurationIsValid();
        }
        
        [Test, CommonAutoData]
        public static void Map_CatalogueItemToAdditionalInformationModel_ResultAsExpected(
            CatalogueItem catalogueItem)
        {
            var clientApplication = 
                JsonConvert.DeserializeObject<ClientApplication>(catalogueItem.Solution.ClientApplication);
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ClientApplicationTypeProfile>();
                cfg.AddProfile<OrganisationProfile>();
            }).CreateMapper();
            
            var actual = mapper.Map<CatalogueItem, ClientApplicationTypesModel>(catalogueItem);

            actual.BackLink.Should()
                .Be($"/marketing/supplier/solution/{catalogueItem.CatalogueItemId}");
            actual.BackLinkText.Should().Be("Return to all sections");
            actual.BrowserBased.Should()
                .Be(clientApplication.ClientApplicationTypes.Any(x => x.EqualsIgnoreCase("browser-based")));
            actual.ClientApplication.Should().BeEquivalentTo(clientApplication);
            actual.NativeDesktop.Should()
                .Be(clientApplication.ClientApplicationTypes.Any(x => x.EqualsIgnoreCase("native-desktop")));
            actual.NativeMobile.Should()
                .Be(clientApplication.ClientApplicationTypes.Any(x => x.EqualsIgnoreCase("native-mobile")));
            actual.SolutionId.Should().Be(catalogueItem.CatalogueItemId);
            actual.SupplierId.Should().Be(catalogueItem.Supplier.Id);
        }

        [Test, CommonAutoData]
        public static void Map_CatalogueItemToBrowserBasedModel_ResultAsExpected(
            CatalogueItem catalogueItem)
        {
            var clientApplication = 
                JsonConvert.DeserializeObject<ClientApplication>(catalogueItem.Solution.ClientApplication);
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ClientApplicationTypeProfile>();
                cfg.AddProfile<OrganisationProfile>();
            }).CreateMapper();

            var actual = mapper.Map<CatalogueItem, BrowserBasedModel>(catalogueItem);
            
            actual.BackLink.Should()
                .Be($"/marketing/supplier/solution/{catalogueItem.CatalogueItemId}");
            actual.BackLinkText.Should().Be("Return to all sections");
            actual.ClientApplication.Should().BeEquivalentTo(clientApplication);
            actual.SolutionId.Should().Be(catalogueItem.CatalogueItemId);
            actual.SupplierId.Should().Be(catalogueItem.Supplier.Id);
        }
        
        [Test, CommonAutoData]
        public static void Map_CatalogueItemToNativeDesktopModel_ResultAsExpected(
            CatalogueItem catalogueItem)
        {
            var clientApplication = 
                JsonConvert.DeserializeObject<ClientApplication>(catalogueItem.Solution.ClientApplication);
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ClientApplicationTypeProfile>();
                cfg.AddProfile<OrganisationProfile>();
            }).CreateMapper();

            var actual = mapper.Map<CatalogueItem, NativeDesktopModel>(catalogueItem);
            
            actual.BackLink.Should()
                .Be($"/marketing/supplier/solution/{catalogueItem.CatalogueItemId}");
            actual.BackLinkText.Should().Be("Return to all sections");
            actual.ClientApplication.Should().BeEquivalentTo(clientApplication);
            actual.SolutionId.Should().Be(catalogueItem.CatalogueItemId);
            actual.SupplierId.Should().Be(catalogueItem.Supplier.Id);
        }

        [Test, CommonAutoData]
        public static void Map_CatalogueItemToNativeMobileModel_ResultAsExpected(
            CatalogueItem catalogueItem)
        {
            var clientApplication = 
                JsonConvert.DeserializeObject<ClientApplication>(catalogueItem.Solution.ClientApplication);
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ClientApplicationTypeProfile>();
                cfg.AddProfile<OrganisationProfile>();
            }).CreateMapper();

            var actual = mapper.Map<CatalogueItem, NativeMobileModel>(catalogueItem);
            
            actual.BackLink.Should()
                .Be($"/marketing/supplier/solution/{catalogueItem.CatalogueItemId}");
            actual.BackLinkText.Should().Be("Return to all sections");
            actual.ClientApplication.Should().BeEquivalentTo(clientApplication);
            actual.SolutionId.Should().Be(catalogueItem.CatalogueItemId);
            actual.SupplierId.Should().Be(catalogueItem.Supplier.Id);
        }
        
        [Test, CommonAutoData]
        public static void Map_ClientApplicationTypesModelToClientApplication_ResultAsExpected(
            ClientApplication clientApplication, ClientApplicationTypesModel model)
        {
            var original = clientApplication.CopyObjectToNew();
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ClientApplicationTypeProfile>();
                cfg.AddProfile<OrganisationProfile>();
            }).CreateMapper();

            mapper.Map(model, clientApplication);
            
            clientApplication.ClientApplicationTypes.Contains("browser-based")
                .Should().Be(model.BrowserBased);
            clientApplication.ClientApplicationTypes.Contains("native-desktop")
                .Should().Be(model.NativeDesktop);
            clientApplication.ClientApplicationTypes.Contains("native-mobile")
                .Should().Be(model.NativeMobile);
            clientApplication.ValidateAllPropertiesExcept(original,
                new[] {nameof(ClientApplication.ClientApplicationTypes)});
        }
    }
}