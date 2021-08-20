﻿using System;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.Test.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.DesktopBasedModels;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Admin.Models.DesktopBasedModels
{
    public static class ThirdPartyComponentsModelTests
    {
        [Theory]
        [CommonAutoData]
        public static void FromCatalogueItem_ValidCatalogueItem_PropertiesSetAsExpected(
            CatalogueItem catalogueItem)
        {
            var actual = new ThirdPartyComponentsModel(catalogueItem);

            actual.ThirdPartyComponents.Should().Be(catalogueItem.Solution.GetClientApplication().NativeDesktopThirdParty.ThirdPartyComponents);
            actual.DeviceCapabilities.Should().Be(catalogueItem.Solution.GetClientApplication().NativeDesktopThirdParty.DeviceCapabilities);
            actual.BackLink.Should().Be($"/admin/catalogue-solutions/manage/{catalogueItem.Id}/client-application-type/desktop");
        }

        [Fact]
        public static void FromCatalogueItem_NullCatalogueItem_ThrowsException()
        {
            var actual = Assert.Throws<ArgumentNullException>(() => new ThirdPartyComponentsModel(null));

            actual.ParamName.Should().Be("catalogueItem");
        }

        [Theory]
        [InlineData("", "", false)]
        [InlineData(" ", "", false)]
        [InlineData(null, "", false)]
        [InlineData("", " ", false)]
        [InlineData("", null, false)]
        [InlineData("Component", null, true)]
        [InlineData("Component", "Capability", true)]
        [InlineData(null, "Capability", true)]
        public static void IsComplete_CorrectlySet(
            string components,
            string capabilities,
            bool expectedCompletionState)
        {
            var model = new ThirdPartyComponentsModel { ThirdPartyComponents = components, DeviceCapabilities = capabilities };

            var actual = model.IsComplete;

            actual.Should().Be(expectedCompletionState);
        }
    }
}