﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Objects.Ordering;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils.TestBases;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Order.Controllers;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.Ordering.AdditionalServices
{
    public class SelectAdditionalServices : BuyerTestBase, IClassFixture<LocalWebApplicationFactory>, IDisposable
    {
        private const string InternalOrgId = "CG-03F";
        private const int OrderId = 90005;
        private static readonly CallOffId CallOffId = new(OrderId, 1);

        private static readonly Dictionary<string, string> Parameters = new()
        {
            { nameof(InternalOrgId), InternalOrgId },
            { nameof(CallOffId), $"{CallOffId}" },
        };

        public SelectAdditionalServices(LocalWebApplicationFactory factory)
            : base(factory, typeof(AdditionalServicesController), nameof(AdditionalServicesController.SelectAdditionalServices), Parameters)
        {
        }

        [Fact]
        public void SelectAdditionalServices_AllSectionsDisplayed()
        {
            CommonActions.PageTitle().Should().BeEquivalentTo($"Additional Services - Order {CallOffId}".FormatForComparison());
            CommonActions.GoBackLinkDisplayed().Should().BeTrue();
            CommonActions.ElementIsDisplayed(AdditionalServicesObjects.ExistingServices).Should().BeFalse();
            CommonActions.ElementIsDisplayed(AdditionalServicesObjects.ServicesToSelect).Should().BeTrue();
            CommonActions.ElementIsDisplayed(AdditionalServicesObjects.NothingToSelect).Should().BeFalse();
            CommonActions.SaveButtonDisplayed().Should().BeTrue();
        }

        [Fact]
        public void SelectAdditionalServices_ClickGoBackLink_ExpectedResult()
        {
            CommonActions.ClickGoBackLink();

            CommonActions.PageLoadedCorrectGetIndex(
                typeof(OrderController),
                nameof(OrderController.Order)).Should().BeTrue();
        }

        [Fact]
        public void SelectAdditionalServices_WithSomeExistingServices_ExpectedResult()
        {
            CommonActions.ClickFirstCheckbox();
            CommonActions.ClickSave();

            NavigateToUrl(
                typeof(AdditionalServicesController),
                nameof(AdditionalServicesController.SelectAdditionalServices),
                Parameters);

            CommonActions.ElementIsDisplayed(AdditionalServicesObjects.ExistingServices).Should().BeTrue();
            CommonActions.ElementIsDisplayed(AdditionalServicesObjects.ServicesToSelect).Should().BeTrue();
            CommonActions.ElementIsDisplayed(AdditionalServicesObjects.NothingToSelect).Should().BeFalse();
        }

        [Fact]
        public void SelectAdditionalServices_WithAllExistingServices_ExpectedResult()
        {
            CommonActions.ClickAllCheckboxes();
            CommonActions.ClickSave();

            NavigateToUrl(
                typeof(AdditionalServicesController),
                nameof(AdditionalServicesController.SelectAdditionalServices),
                Parameters);

            CommonActions.ElementIsDisplayed(AdditionalServicesObjects.ExistingServices).Should().BeTrue();
            CommonActions.ElementIsDisplayed(AdditionalServicesObjects.ServicesToSelect).Should().BeFalse();
            CommonActions.ElementIsDisplayed(AdditionalServicesObjects.NothingToSelect).Should().BeTrue();
        }

        [Fact]
        public void SelectAdditionalServices_NoSelectionMade_ExpectedResult()
        {
            CommonActions.ClickSave();

            CommonActions.PageLoadedCorrectGetIndex(
                typeof(OrderController),
                nameof(OrderController.Order)).Should().BeTrue();
        }

        [Fact]
        public void SelectAdditionalServices_SelectionMade_ExpectedResult()
        {
            GetAdditionalServices().Count.Should().Be(0);

            CommonActions.ClickFirstCheckbox();
            CommonActions.ClickSave();

            CommonActions.PageLoadedCorrectGetIndex(
                typeof(ServiceRecipientsController),
                nameof(ServiceRecipientsController.AdditionalServiceRecipients)).Should().BeTrue();

            GetAdditionalServices().Count.Should().Be(1);
        }

        public void Dispose()
        {
            var context = GetEndToEndDbContext();

            foreach (var service in GetAdditionalServices())
            {
                context.OrderItems.Remove(service);
            }

            context.SaveChanges();
        }

        private List<OrderItem> GetAdditionalServices()
        {
            var context = GetEndToEndDbContext();

            return context.OrderItems
                .Where(x => x.OrderId == OrderId
                    && x.CatalogueItem.CatalogueItemType == CatalogueItemType.AdditionalService)
                .ToList();
        }
    }
}
