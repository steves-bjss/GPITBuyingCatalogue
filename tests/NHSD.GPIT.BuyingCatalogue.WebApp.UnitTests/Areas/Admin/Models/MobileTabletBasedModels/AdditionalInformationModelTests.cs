﻿using System;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.Test.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.MobileTabletBasedModels;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Admin.Models.MobileTabletBasedModels
{
    public static class AdditionalInformationModelTests
    {
        [Theory]
        [CommonAutoData]
        public static void FromCatalogueItem_ValidCatalogueItem_PropertiesSetAsExpected(
            CatalogueItem catalogueItem)
        {
            var actual = new AdditionalInformationModel(catalogueItem);

            actual.AdditionalInformation.Should().Be(catalogueItem.Solution.GetClientApplication().NativeMobileAdditionalInformation);
            actual.BackLink.Should().Be($"/admin/catalogue-solutions/manage/{catalogueItem.Id}/client-application-type/mobiletablet");
        }

        [Fact]
        public static void FromCatalogueItem_NullCatalogueItem_ThrowsException()
        {
            var actual = Assert.Throws<ArgumentNullException>(() => new AdditionalInformationModel(null));

            actual.ParamName.Should().Be("catalogueItem");
        }

        [Theory]
        [InlineData("Some Additional Information", true)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData(null, false)]
        public static void IsComplete_CorrectlySet(
            string additionalInformation,
            bool expectedCompletionState)
        {
            var model = new AdditionalInformationModel { AdditionalInformation = additionalInformation };

            var actual = model.IsComplete;

            actual.Should().Be(expectedCompletionState);
        }
    }
}