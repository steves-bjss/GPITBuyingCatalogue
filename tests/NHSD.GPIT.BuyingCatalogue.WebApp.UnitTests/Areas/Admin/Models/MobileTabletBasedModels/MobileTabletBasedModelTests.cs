﻿using System;
using System.Text.Json;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;
using NHSD.GPIT.BuyingCatalogue.Test.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.MobileTabletBasedModels;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Admin.Models.MobileTabletBasedModels
{
    public static class MobileTabletBasedModelTests
    {
        [Theory]
        [CommonAutoData]
        public static void FromCatalogueItem_ValidCatalogueItem_NoExistingNativeMobile_BackLinkSetCorrectly(
            CatalogueItem catalogueItem,
            ClientApplication clientApplication)
        {
            clientApplication.ClientApplicationTypes.Clear();
            clientApplication.ClientApplicationTypes.Add("browser-based");
            catalogueItem.Solution.ClientApplication = JsonSerializer.Serialize(clientApplication);

            var actual = new MobileTabletBasedModel(catalogueItem);
            actual.BackLink.Should().Be($"/admin/catalogue-solutions/manage/{catalogueItem.Id}/client-application-type/add-application-type");
        }

        [Theory]
        [CommonAutoData]
        public static void FromCatalogueItem_ValidCatalogueItem_WithNativeMobile_BackLinkSetCorrectly(
            CatalogueItem catalogueItem,
            ClientApplication clientApplication)
        {
            clientApplication.ClientApplicationTypes.Clear();
            clientApplication.ClientApplicationTypes.Add("native-mobile");
            catalogueItem.Solution.ClientApplication = JsonSerializer.Serialize(clientApplication);

            var actual = new MobileTabletBasedModel(catalogueItem);

            actual.BackLink.Should().Be($"/admin/catalogue-solutions/manage/{catalogueItem.Id}/client-application-type");
        }

        [Fact]
        public static void FromCatalogueItem_NullCatalogueItem_ThrowsException()
        {
            var actual = Assert.Throws<ArgumentNullException>(() => new MobileTabletBasedModel(null));

            actual.ParamName.Should().Be("catalogueItem");
        }
    }
}