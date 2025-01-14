﻿using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Framework.Actions.Common;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.RegressionTests.Utils;
using OpenQA.Selenium;

namespace NHSD.GPIT.BuyingCatalogue.RegressionTests.Pages.Competitions.View_Result
{
    public class ViewCompetitionResults : PageBase
    {
        public ViewCompetitionResults(IWebDriver driver, CommonActions commonActions)
            : base(driver, commonActions)
        {
        }

        public void ViewResults()
        {
            CommonActions.ClickSave();
            CommonActions.HintText().Should().Be("These are the results for this competition.".FormatForComparison());
        }

        public void ViewMultipleWinningResults()
        {
            Driver.Navigate().Refresh();
            CommonActions.InsetText().Should().Be("Information:Your competition has produced more than 1 solution with a winning score. You can therefore choose to procure any of the winning solutions.".FormatForComparison());
        }
    }
}
