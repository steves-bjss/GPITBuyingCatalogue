﻿using NHSD.GPIT.BuyingCatalogue.E2ETests.Objects.Common;
using OpenQA.Selenium;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Objects.Ordering
{
    internal static class CalloffPartyInformation
    {
        public static By OrganisationName => ByExtensions.DataTestId("organisation-name");

        public static By OrganisationOdsCode => ByExtensions.DataTestId("organisation-ods-code");

        public static By OrganisationAddress => ByExtensions.DataTestId("organisation-address");

        public static By FirstNameInput => By.Id("Contact_FirstName");

        public static By LastNameInput => By.Id("Contact_LastName");

        public static By EmailAddressInput => By.Id("Contact_EmailAddress");

        public static By PhoneNumberInput => By.Id("Contact_TelephoneNumber");
    }
}
