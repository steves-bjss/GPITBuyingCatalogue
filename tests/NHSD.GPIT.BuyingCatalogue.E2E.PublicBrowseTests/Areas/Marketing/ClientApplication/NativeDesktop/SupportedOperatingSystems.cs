﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Actions.Common;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using System.Threading.Tasks;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.Marketing.ClientApplication.NativeDesktop
{
    public sealed class SupportedOperatingSystems : TestBase, IClassFixture<LocalWebApplicationFactory>
    {
        public SupportedOperatingSystems(LocalWebApplicationFactory factory) : base(factory, "marketing/supplier/solution/99999-99/section/native-desktop/operating-systems")
        {
            ClearClientApplication("99999-99");
            driver.Navigate().Refresh();
        }

        [Fact]
        public async Task SupportedOperatingSystems_CompleteAllFields()
        {
            var operatingSystems = TextGenerators.TextInputAddText(CommonSelectors.SupportedOperatingSystemDescription, 1000);

            CommonActions.ClickSave();

            using var context = GetBCContext();

            var clientApplication = (await context.Solutions.SingleAsync(s => s.Id == "99999-99")).ClientApplication;
            clientApplication.Should().ContainEquivalentOf($"\"NativeDesktopOperatingSystemsDescription\":\"{operatingSystems}\"");
        }

        [Fact]
        public void SupportedOperatingSystems_SectionComplete()
        {
            TextGenerators.TextInputAddText(CommonSelectors.SupportedOperatingSystemDescription, 1000);

            CommonActions.ClickSave();

            MarketingPages.DashboardActions.SectionMarkedComplete("Supported operating systems").Should().BeTrue();
        }

        [Fact]
        public void SupportedOperatingSystems_SectionIncomplete()
        {
            CommonActions.ClickGoBackLink();

            MarketingPages.DashboardActions.SectionMarkedComplete("Supported operating systems").Should().BeFalse();
        }
    }
}
