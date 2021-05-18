﻿using OpenQA.Selenium;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Objects.Marketing
{
    internal static class CommonSelectors
    {
        internal static By SaveAndReturn => By.Id("Submit");

        internal static By GoBackLink => By.ClassName("nhsuk-back-link__link");

        internal static By Link => By.Id("Link");

        internal static By Description => By.Id("Description");

        internal static By Summary => By.Id("Summary");

        internal static By Checkbox => By.CssSelector("input[type=checkbox]");

        internal static By NhsInput => By.ClassName("nhsuk-input");

        internal static By NhsErrorSection => By.ClassName("nhsuk-error-summary");

        internal static By AdditionalInfoTextArea => By.Id("AdditionalInformation");

        internal static By RadioButtonItems => By.CssSelector(".nhsuk-radios__item");

        internal static By BrowserCheckboxItem => By.CssSelector(".nhsuk-checkboxes__item");

        internal static By BrowserBasedCheckbox => By.Id("BrowserBased");

        internal static By NativeMobileCheckbox => By.Id("NativeMobile");

        internal static By NativeDesktopCheckbox => By.Id("NativeDesktop");

        internal static By ConnectionSpeedSelect => By.Id("SelectedConnectionSpeed");

        internal static By ResolutionSelect => By.Id("SelectedScreenResolution");

        internal static By MemorySelect => By.Id("SelectedMemorySize");

        internal static By ThirdPartyComponentTextArea => By.Id("ThirdPartyComponents");

        internal static By DeviceCapabilityTextArea => By.Id("DeviceCapabilities");

        internal static By SupportedOperatingSystemDescription => By.Id("OperatingSystemsDescription");

        internal static By StorageDescriptionTextArea => By.Id("StorageDescription");

        internal static By MinimumCpuTextArea => By.Id("MinimumCpu");

        internal static By Header3 => By.TagName("h3");
    }
}