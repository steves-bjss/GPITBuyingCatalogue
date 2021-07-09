﻿using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.Admin
{
    public sealed class AdminDashboardNew : TestBase, IClassFixture<LocalWebApplicationFactory>
    {
        public AdminDashboardNew(LocalWebApplicationFactory factory)
            : base(factory, "admin")
        {
            Login();
        }

        [Fact]
        public void ManageSuppliers_ManageSuppliersOrgLinkDisplayed()
        {
            AdminPages.AddSolution.ManageSuppliersLinkDisplayed().Should().BeTrue();
        }

        [Fact]
        public void ManageCatalogueSolutions_AddSolutionLinkDisplayed()
        {
            AdminPages.AddSolution.AddSolutionLinkDisplayed().Should().BeTrue();
        }
    }
}