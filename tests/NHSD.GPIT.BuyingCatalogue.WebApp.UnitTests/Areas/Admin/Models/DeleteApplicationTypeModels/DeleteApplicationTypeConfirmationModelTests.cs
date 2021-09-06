﻿using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
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
            CatalogueItemId catalogueItemId)
        {
            var actual = new DeleteApplicationTypeConfirmationModel(catalogueItemId, ClientApplicationType.BrowserBased);
            actual.BackLinkText.Should().Equals("Go back");
            actual.BackLink.Should().Be($"/admin/catalogue-solutions/manage/{catalogueItemId}/client-application-type/browser-based");
            actual.ApplicationType.Should().Be("browser-based");
        }

        [Theory]
        [CommonAutoData]
        public static void Desktop_PropertiesSetCorrectly(
            CatalogueItemId catalogueItemId)
        {
            var actual = new DeleteApplicationTypeConfirmationModel(catalogueItemId, ClientApplicationType.Desktop);
            actual.BackLinkText.Should().Equals("Go back");
            actual.BackLink.Should().Be($"/admin/catalogue-solutions/manage/{catalogueItemId}/client-application-type/desktop");
            actual.ApplicationType.Should().Be("desktop");
        }

        [Theory]
        [CommonAutoData]
        public static void Mobile_PropertiesSetCorrectly(
            CatalogueItemId catalogueItemId)
        {
            var actual = new DeleteApplicationTypeConfirmationModel(catalogueItemId, ClientApplicationType.MobileTablet);
            actual.BackLinkText.Should().Equals("Go back");
            actual.BackLink.Should().Be($"/admin/catalogue-solutions/manage/{catalogueItemId}/client-application-type/mobiletablet");
            actual.ApplicationType.Should().Be("mobile or tablet");
        }
    }
}