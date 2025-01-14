﻿using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Framework.Actions.Common;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Framework.Objects.Ordering.SolutionSelection;
using NHSD.GPIT.BuyingCatalogue.RegressionTests.Utils;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Orders.Controllers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Orders.Controllers.SolutionSelection;
using OpenQA.Selenium;

namespace NHSD.GPIT.BuyingCatalogue.RegressionTests.Pages.Ordering.StepTwo.SolutionSelection
{
    public class SelectEditAssociatedService : PageBase
    {
        public SelectEditAssociatedService(IWebDriver driver, CommonActions commonActions, LocalWebApplicationFactory factory)
            : base(driver, commonActions)
        {
            Factory = factory;
        }

        internal LocalWebApplicationFactory Factory { get; private set; }

        public void AddAssociatedService(string preference = "No", string associatedService = "")
        {
            CommonActions.ClickRadioButtonWithText(preference);

            CommonActions.ClickSave();

            if (preference == "Yes")
            {
                CommonActions.PageLoadedCorrectGetIndex(
                 typeof(AssociatedServicesController),
                 nameof(AssociatedServicesController.SelectAssociatedServices)).Should().BeTrue();

                CommonActions.ClickCheckboxByLabel(associatedService);

                CommonActions.ClickSave();
            }
        }

        public void EditAssociatedService(string solutionName, IEnumerable<string> newAssociatedServices, bool hasTheOrderAssociatedService, IEnumerable<string>? oldAssociatedServices)
        {
            if (SolutionHasAssociatedService(solutionName))
            {
                if (hasTheOrderAssociatedService)
                {
                    if (oldAssociatedServices != default && oldAssociatedServices.All(a => !string.IsNullOrWhiteSpace(a)))
                    {
                        foreach (var oldAssociatedService in oldAssociatedServices)
                        {
                            CommonActions.ClickLinkElement(ReviewSolutionsObjects.RemoveSolutionService(oldAssociatedService));
                            CommonActions.PageLoadedCorrectGetIndex(
                            typeof(CatalogueSolutionsController),
                            nameof(CatalogueSolutionsController.RemoveService)).Should().BeTrue();
                            var removeService = $"Yes, I confirm I want to remove {oldAssociatedService}";

                            CommonActions.ClickRadioButtonWithText(removeService);
                            CommonActions.ClickSave();
                        }
                    }
                }

                if (newAssociatedServices != default && newAssociatedServices.All(a => !string.IsNullOrWhiteSpace(a)))
                {
                    CommonActions.ClickLinkElement(ReviewSolutionsObjects.AddAssociatedServiceLink);

                    CommonActions.PageLoadedCorrectGetIndex(
                      typeof(AssociatedServicesController),
                      nameof(AssociatedServicesController.SelectAssociatedServices)).Should().BeTrue();
                    foreach (var newAssociatedService in newAssociatedServices)
                    {
                        CommonActions.ClickCheckboxByLabel(newAssociatedService);
                    }
                }

                CommonActions.ClickSave();
            }
            else
            {
                CommonActions.ClickSaveAndContinue();

                CommonActions.PageLoadedCorrectGetIndex(
                  typeof(OrderController),
                  nameof(OrderController.Order)).Should().BeTrue();
            }
        }

        private bool SolutionHasAssociatedService(string solutionName)
        {
            using var dbContext = Factory.DbContext;
            var result = dbContext.AssociatedServices.Any(a => a.CatalogueItem.Solution.CatalogueItem.Name == solutionName);
            return result;
        }
    }
}
