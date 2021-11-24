﻿using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;
using NHSD.GPIT.BuyingCatalogue.Test.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.DeleteApplicationTypeModels;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Admin.Models.DeleteApplicationTypeModels
{
    public static class DeleteApplicationTypeConfirmationModelTests
    {
        [Theory]
        [CommonAutoData]
        public static void BrowserBased_PropertiesSetCorrectly(
            CatalogueItem catalogueItem)
        {
            var actual = new DeleteApplicationTypeConfirmationModel(catalogueItem, ClientApplicationType.BrowserBased);
            actual.ApplicationType.Should().Be("browser-based");
        }

        [Theory]
        [CommonAutoData]
        public static void Desktop_PropertiesSetCorrectly(
            CatalogueItem catalogueItem)
        {
            var actual = new DeleteApplicationTypeConfirmationModel(catalogueItem, ClientApplicationType.Desktop);
            actual.ApplicationType.Should().Be("desktop");
        }

        [Theory]
        [CommonAutoData]
        public static void Mobile_PropertiesSetCorrectly(
            CatalogueItem catalogueItem)
        {
            var actual = new DeleteApplicationTypeConfirmationModel(catalogueItem, ClientApplicationType.MobileTablet);
            actual.ApplicationType.Should().Be("mobile or tablet");
        }
    }
}
