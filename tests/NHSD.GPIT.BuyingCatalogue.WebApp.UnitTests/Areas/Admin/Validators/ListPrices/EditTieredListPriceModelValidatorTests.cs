﻿using System.Collections.Generic;
using AutoFixture.Xunit2;
using FluentValidation.TestHelper;
using Moq;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.ListPrice;
using NHSD.GPIT.BuyingCatalogue.UnitTest.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.ListPriceModels;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Validators.ListPrices;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Admin.Validators.ListPrices
{
    public static class EditTieredListPriceModelValidatorTests
    {
        [Theory]
        [CommonAutoData]
        public static void Validate_NoTiers_SetsModelError(
            Solution solution,
            CataloguePrice price,
            EditTieredListPriceModel model,
            [Frozen] Mock<IListPriceService> service,
            EditTieredListPriceModelValidator validator)
        {
            model.SelectedPublicationStatus = PublicationStatus.Published;

            price.CataloguePriceTiers = new HashSet<CataloguePriceTier>();
            solution.CatalogueItem.CataloguePrices.Add(price);

            model.CatalogueItemId = solution.CatalogueItemId;
            model.CataloguePriceId = price.CataloguePriceId;

            service.Setup(s => s.GetCatalogueItemWithListPrices(solution.CatalogueItemId))
                .ReturnsAsync(solution.CatalogueItem);

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(m => m.SelectedPublicationStatus)
                .WithErrorMessage(SharedListPriceValidationErrors.MissingTiersError);
        }

        [Theory]
        [CommonAutoData]
        public static void Validate_InvalidStartingRange_SetsModelError(
            Solution solution,
            CataloguePrice price,
            CataloguePriceTier tier,
            EditTieredListPriceModel model,
            [Frozen] Mock<IListPriceService> service,
            EditTieredListPriceModelValidator validator)
        {
            model.SelectedPublicationStatus = PublicationStatus.Published;

            tier.LowerRange = 2;
            tier.UpperRange = null;

            price.CataloguePriceTiers = new HashSet<CataloguePriceTier>
            {
                tier,
            };

            solution.CatalogueItem.CataloguePrices.Add(price);

            model.CatalogueItemId = solution.CatalogueItemId;
            model.CataloguePriceId = price.CataloguePriceId;

            service.Setup(s => s.GetCatalogueItemWithListPrices(solution.CatalogueItemId))
                .ReturnsAsync(solution.CatalogueItem);

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(m => m.SelectedPublicationStatus)
                .WithErrorMessage(SharedListPriceValidationErrors.InvalidStartingRangeError);
        }

        [Theory]
        [CommonAutoData]
        public static void Validate_NoInfiniteRange_SetsModelError(
            Solution solution,
            CataloguePrice price,
            CataloguePriceTier tier,
            EditTieredListPriceModel model,
            [Frozen] Mock<IListPriceService> service,
            EditTieredListPriceModelValidator validator)
        {
            model.SelectedPublicationStatus = PublicationStatus.Published;

            tier.LowerRange = 1;
            tier.UpperRange = 9;

            price.CataloguePriceTiers = new HashSet<CataloguePriceTier>
            {
                tier,
            };

            solution.CatalogueItem.CataloguePrices.Add(price);

            model.CatalogueItemId = solution.CatalogueItemId;
            model.CataloguePriceId = price.CataloguePriceId;

            service.Setup(s => s.GetCatalogueItemWithListPrices(solution.CatalogueItemId))
                .ReturnsAsync(solution.CatalogueItem);

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(m => m.SelectedPublicationStatus)
                .WithErrorMessage(SharedListPriceValidationErrors.InvalidEndingRangeError);
        }

        [Theory]
        [CommonAutoData]
        public static void Validate_GapsBetweenRanges_SetsModelError(
            Solution solution,
            CataloguePrice price,
            CataloguePriceTier firstTier,
            CataloguePriceTier secondTier,
            EditTieredListPriceModel model,
            [Frozen] Mock<IListPriceService> service,
            EditTieredListPriceModelValidator validator)
        {
            model.SelectedPublicationStatus = PublicationStatus.Published;

            firstTier.LowerRange = 1;
            firstTier.UpperRange = 9;

            secondTier.LowerRange = 11;
            secondTier.UpperRange = null;

            price.CataloguePriceTiers = new HashSet<CataloguePriceTier>
            {
                firstTier,
                secondTier,
            };

            solution.CatalogueItem.CataloguePrices.Add(price);

            model.CatalogueItemId = solution.CatalogueItemId;
            model.CataloguePriceId = price.CataloguePriceId;

            service.Setup(s => s.GetCatalogueItemWithListPrices(solution.CatalogueItemId))
                .ReturnsAsync(solution.CatalogueItem);

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(m => m.SelectedPublicationStatus)
                .WithErrorMessage(string.Format(SharedListPriceValidationErrors.RangeGapError, "1", "2"));
        }

        [Theory]
        [CommonAutoData]
        public static void Validate_OverlapBetweenRanges_SetsModelError(
            Solution solution,
            CataloguePrice price,
            CataloguePriceTier firstTier,
            CataloguePriceTier secondTier,
            EditTieredListPriceModel model,
            [Frozen] Mock<IListPriceService> service,
            EditTieredListPriceModelValidator validator)
        {
            model.SelectedPublicationStatus = PublicationStatus.Published;

            firstTier.LowerRange = 1;
            firstTier.UpperRange = 9;

            secondTier.LowerRange = 8;
            secondTier.UpperRange = null;

            price.CataloguePriceTiers = new HashSet<CataloguePriceTier>
            {
                firstTier,
                secondTier,
            };

            solution.CatalogueItem.CataloguePrices.Add(price);

            model.CatalogueItemId = solution.CatalogueItemId;
            model.CataloguePriceId = price.CataloguePriceId;

            service.Setup(s => s.GetCatalogueItemWithListPrices(solution.CatalogueItemId))
                .ReturnsAsync(solution.CatalogueItem);

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(m => m.SelectedPublicationStatus)
                .WithErrorMessage(string.Format(SharedListPriceValidationErrors.RangeOverlapError, "1", "2"));
        }

        [Theory]
        [CommonAutoData]
        public static void Validate_Valid_NoModelErrors(
            Solution solution,
            CataloguePrice price,
            CataloguePriceTier firstTier,
            CataloguePriceTier secondTier,
            CataloguePriceTier thirdTier,
            EditTieredListPriceModel model,
            [Frozen] Mock<IListPriceService> service,
            EditTieredListPriceModelValidator validator)
        {
            firstTier.LowerRange = 1;
            firstTier.UpperRange = 9;

            secondTier.LowerRange = 10;
            secondTier.UpperRange = 19;

            thirdTier.LowerRange = 20;
            thirdTier.UpperRange = null;

            price.CataloguePriceTiers = new HashSet<CataloguePriceTier>();
            price.CataloguePriceTiers.Add(firstTier);
            price.CataloguePriceTiers.Add(secondTier);

            solution.CatalogueItem.CataloguePrices.Add(price);

            model.CatalogueItemId = solution.CatalogueItemId;
            model.CataloguePriceId = price.CataloguePriceId;

            service.Setup(s => s.GetCatalogueItemWithListPrices(solution.CatalogueItemId))
                .ReturnsAsync(solution.CatalogueItem);

            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
