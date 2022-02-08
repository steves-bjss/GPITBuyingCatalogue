﻿using NHSD.GPIT.BuyingCatalogue.E2ETests.Objects.Common;
using OpenQA.Selenium;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Objects.Admin.SupplierDefinedEpics
{
    internal static class SupplierDefinedEpicsDashboardObjects
    {
        internal static By EpicsTable => ByExtensions.DataTestId("sde-table");

        internal static By AddEpicLink => By.LinkText("Add a supplier defined Epic");

        internal static By InactiveItemsContainer => By.Id("inactive-items-checkbox");

        internal static By InactiveItemRow => By.ClassName("inactive");
    }
}