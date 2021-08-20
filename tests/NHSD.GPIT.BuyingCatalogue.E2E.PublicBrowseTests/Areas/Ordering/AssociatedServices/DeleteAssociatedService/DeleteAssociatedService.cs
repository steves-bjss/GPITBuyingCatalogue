﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils.TestBases;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Order.Controllers;
using OpenQA.Selenium;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.Ordering.AssociatedServices.DeleteAssociatedService
{
    public sealed class DeleteAssociatedService
        : BuyerTestBase, IClassFixture<LocalWebApplicationFactory>, IAsyncLifetime
    {
        private static readonly CallOffId CallOffId = new(90008, 1);
        private static readonly CatalogueItemId CatalogueItemId = new(99998, "-S-999");
        private static readonly string OdsCode = "03F";
        private static readonly string CatalogueItemName = "E2E Single Price Added Associated Service";

        private static readonly Dictionary<string, string> Parameters =
            new()
            {
                { nameof(OdsCode), OdsCode },
                { nameof(CallOffId), CallOffId.ToString() },
                { nameof(CatalogueItemId), CatalogueItemId.ToString() },
                { nameof(CatalogueItemName), CatalogueItemName },
            };

        public DeleteAssociatedService(LocalWebApplicationFactory factory)
        : base(
              factory,
              typeof(AssociatedServicesController),
              nameof(AssociatedServicesController.SelectAssociatedService),
              Parameters)
        {
        }

        [Fact]
        public void DeleteAssociatedService_AllSectionsDisplayed()
        {
            CommonActions.SaveButtonDisplayed().Should().BeTrue();

            CommonActions.ElementIsDisplayed(
                Objects.Ordering.CatalogueSolutions.CatalogueSolutionsDeleteSolutionCancelLink).Should().BeTrue();
        }

        [Fact]
        public void DeleteAssociatedService_CancelDelete_ExpectedResult()
        {
            CommonActions.ClickLinkElement(Objects.Ordering.CatalogueSolutions.CatalogueSolutionsDeleteSolutionCancelLink);

            CommonActions.PageLoadedCorrectGetIndex(
                typeof(AssociatedServicesController),
                nameof(AssociatedServicesController.EditAssociatedService))
                .Should()
                .BeTrue();
        }

        [Fact]
        public void DeleteAssociatedService_DeleteSolution_ExpectedResult()
        {
            CommonActions.ClickSave();

            CommonActions.PageLoadedCorrectGetIndex(
                typeof(AssociatedServicesController),
                nameof(AssociatedServicesController.Index))
                .Should()
                .BeTrue();

            CommonActions.ElementIsDisplayed(By.LinkText(CatalogueItemName)).Should().BeFalse();

            Driver
                .FindElements(Objects.Ordering.CatalogueSolutions.CatalogueSolutionsAnySolutionRow)
                .Any()
                .Should()
                .BeFalse();
        }

        public Task InitializeAsync()
        {
            InitializeSessionHandler();

            NavigateToUrl(
                typeof(DeleteAssociatedServiceController),
                nameof(DeleteAssociatedServiceController.DeleteAssociatedService),
                Parameters);

            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return DisposeSession();
        }
    }
}