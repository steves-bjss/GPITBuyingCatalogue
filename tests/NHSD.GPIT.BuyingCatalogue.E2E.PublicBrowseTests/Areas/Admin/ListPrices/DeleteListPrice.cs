﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Framework.Objects.Admin;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Framework.Objects.Common;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils.TestBases;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Controllers;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.Admin.ListPrices
{
    public sealed class DeleteListPrice : AuthorityTestBase, IClassFixture<LocalWebApplicationFactory>
    {
        private const int ListPriceId = 18;
        private static readonly CatalogueItemId SolutionId = new(99999, "001");

        private static readonly Dictionary<string, string> Parameters = new()
        {
            { nameof(SolutionId), SolutionId.ToString() },
            { nameof(ListPriceId), ListPriceId.ToString() },
        };

        public DeleteListPrice(LocalWebApplicationFactory factory)
            : base(
                  factory,
                  typeof(ListPriceController),
                  nameof(ListPriceController.DeleteListPrice),
                  Parameters)
        {
        }

        [Fact]
        public void DeleteListPrice_AllSectionsDisplayed()
        {
            CommonActions.GoBackLinkDisplayed().Should().BeTrue();
            CommonActions.SaveButtonDisplayed().Should().BeTrue();

            CommonActions
                .ElementIsDisplayed(CommonSelectors.Header1)
                .Should()
                .BeTrue();

            CommonActions
                .ElementIsDisplayed(ListPricesObjects.DeleteListPriceCancelLink)
                .Should()
                .BeTrue();
        }

        [Fact]
        public void DeleteListPrice_ClickGoBackLink_ExpectedResult()
        {
            CommonActions.ClickGoBackLink();

            CommonActions.PageLoadedCorrectGetIndex(
                    typeof(ListPriceController),
                    nameof(ListPriceController.EditListPrice))
                .Should()
                .BeTrue();
        }

        [Fact]
        public void DeleteListPrice_ClickCancelLink_ExpectedResult()
        {
            CommonActions.ClickLinkElement(ListPricesObjects.DeleteListPriceCancelLink);

            CommonActions.PageLoadedCorrectGetIndex(
                    typeof(ListPriceController),
                    nameof(ListPriceController.EditListPrice))
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task DeleteListPrice_ClickDelete_ListPriceDeleted()
        {
            await using var context = GetEndToEndDbContext();

            var originalCount = context.CataloguePrices.Count(cp => cp.CatalogueItemId == SolutionId);

            CommonActions.ClickSave();

            CommonActions.PageLoadedCorrectGetIndex(
                    typeof(ListPriceController),
                    nameof(ListPriceController.ManageListPrices))
                .Should()
                .BeTrue();

            context.CataloguePrices.Count(cp => cp.CatalogueItemId == SolutionId).Should().Be(originalCount - 1);
        }
    }
}
