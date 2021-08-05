﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils.TestBases;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Order.Controllers;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.Ordering
{
    public sealed class NewOrderDescription
        : BuyerTestBase, IClassFixture<LocalWebApplicationFactory>
    {
        private static readonly Dictionary<string, string> Parameters = new() { { "OdsCode", "03F" } };

        public NewOrderDescription(LocalWebApplicationFactory factory)
            : base(
                  factory,
                  typeof(OrderDescriptionController),
                  nameof(OrderDescriptionController.NewOrderDescription),
                  Parameters)
        {
        }

        [Fact]
        public void NewOrderDescription_AllSectionsDisplayed()
        {
            CommonActions.PageTitle().Should().BeEquivalentTo("Order description".FormatForComparison());
            CommonActions.GoBackLinkDisplayed().Should().BeTrue();
            CommonActions.SaveButtonDisplayed().Should().BeTrue();
            CommonActions.ElementIsDisplayed(Objects.Ordering.OrderDescription.DescriptionInput).Should().BeTrue();
        }

        [Fact]
        public void NewOrderDescription_NoTextThrowsError()
        {
            CommonActions.ClickSave();

            CommonActions.PageLoadedCorrectGetIndex(
                typeof(OrderDescriptionController),
                nameof(OrderDescriptionController.NewOrderDescription))
                    .Should().BeTrue();

            CommonActions.ErrorSummaryDisplayed().Should().BeTrue();

            CommonActions.ElementShowingCorrectErrorMessage("Description", "Enter a description")
                .Should().BeTrue();
        }

        [Fact]
        public async Task NewOrderDescription_InputText_CreatesOrder()
        {
            var description = TextGenerators.TextInputAddText(Objects.Ordering.OrderDescription.DescriptionInput, 100);

            CommonActions.ClickSave();

            CommonActions.PageLoadedCorrectGetIndex(
                typeof(OrderController),
                nameof(OrderController.Order)).Should().BeTrue();

            await using var context = GetEndToEndDbContext();
            var order = await context.Orders.OrderByDescending(o => o.Created).FirstAsync();

            order.Description.Should().Be(description);
        }
    }
}