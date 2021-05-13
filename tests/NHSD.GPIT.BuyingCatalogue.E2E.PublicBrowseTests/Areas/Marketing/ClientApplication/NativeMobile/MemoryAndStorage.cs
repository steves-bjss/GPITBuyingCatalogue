﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Actions.Common;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using System.Threading.Tasks;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.Marketing.ClientApplication.NativeMobile
{
    public sealed class MemoryAndStorage : TestBase, IClassFixture<LocalWebApplicationFactory>
    {
        public MemoryAndStorage(LocalWebApplicationFactory factory) : base(factory, "marketing/supplier/solution/99999-99/section/native-mobile/memory-and-storage")
        {
            ClearClientApplication("99999-99");
            driver.Navigate().Refresh();
        }

        [Fact]
        public async Task MemoryAndStorage_CompleteAllFields()
        {
            CommonActions.SelectDropdownItem(CommonSelectors.MemorySelect, 1);

            var description = TextGenerators.TextInputAddText(CommonSelectors.Description, 200);

            CommonActions.ClickSave();

            using var context = GetBCContext();

            var clientApplication = (await context.Solutions.SingleAsync(s => s.Id == "99999-99")).ClientApplication;

            clientApplication.Should().ContainEquivalentOf("MinimumMemoryRequirement\":\"256MB\"");
            clientApplication.Should().ContainEquivalentOf($"Description\":\"{description}\"");
        }

        [Fact]
        public void MemoryAndStorage_SectionComplete()
        {
            CommonActions.SelectDropdownItem(CommonSelectors.MemorySelect, 1);

            TextGenerators.TextInputAddText(CommonSelectors.Description, 200);

            CommonActions.ClickSave();

            MarketingPages.DashboardActions.SectionMarkedComplete("Memory and storage").Should().BeTrue();
        }

        [Fact]
        public void MemoryAndStorage_SectionIncomplete()
        {
            CommonActions.ClickGoBackLink();

            MarketingPages.DashboardActions.SectionMarkedComplete("Memory and storage").Should().BeFalse();
        }
    }
}
