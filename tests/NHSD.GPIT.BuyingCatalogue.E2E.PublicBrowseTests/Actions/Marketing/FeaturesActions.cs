﻿using NHSD.GPIT.BuyingCatalogue.E2ETests.Common.Actions;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Objects.Marketing;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils.RandomData;
using OpenQA.Selenium;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Actions.Marketing
{
    internal sealed class FeaturesActions : ActionBase
    {
        public FeaturesActions(IWebDriver driver) : base(driver)
        {
        }

        internal string EnterFeature(int index = 0)
        {
            var randomString = Strings.RandomString(100);

            Driver.FindElements(FeaturesObjects.FeatureInput)[index].SendKeys(randomString);

            return randomString;
        }
    }
}