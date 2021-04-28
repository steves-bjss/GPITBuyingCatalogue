﻿using NHSD.GPIT.BuyingCatalogue.E2ETests.Objects.Common;
using OpenQA.Selenium;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Objects.Marketing
{
    internal static class DashboardObjects
    {
        internal static By SectionTitle => CustomBy.DataTestId("dashboard-section-title");

        internal static By Sections => By.CssSelector("li.bc-c-section-list__item");

        internal static By SectionStatus => CustomBy.DataTestId("dashboard-section-status");
    }
}
