﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils.TestBases;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Controllers;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.Admin.AddNewSolution.ClientApplicationTypes.BrowserBased
{
    public sealed class ConnectivityAndResolution : AuthorityTestBase, IClassFixture<LocalWebApplicationFactory>, IDisposable
    {
        private static readonly CatalogueItemId SolutionId = new(99999, "002");

        private static readonly Dictionary<string, string> Parameters = new()
        {
            { nameof(SolutionId), SolutionId.ToString() },
        };

        public ConnectivityAndResolution(LocalWebApplicationFactory factory)
            : base(
                  factory,
                  typeof(CatalogueSolutionsController),
                  nameof(CatalogueSolutionsController.ConnectivityAndResolution),
                  Parameters)
        {
        }

        [Fact]
        public async Task ConnectivityAndResolution_SavePage()
        {
            var connectionSpeed = CommonActions.SelectRandomDropDownItem(Objects.Admin.AddSolutionObjects.ConnectivityDropdown);

            var resolution = CommonActions.SelectRandomDropDownItem(Objects.Admin.AddSolutionObjects.ResolutionDropdown);

            CommonActions.ClickSave();

            CommonActions.PageLoadedCorrectGetIndex(
                typeof(CatalogueSolutionsController),
                nameof(CatalogueSolutionsController.BrowserBased)).Should().BeTrue();

            await using var context = GetEndToEndDbContext();
            var solution = await context.Solutions.SingleAsync(s => s.CatalogueItemId == SolutionId);

            var clientApplication = JsonSerializer.Deserialize<ServiceContracts.Solutions.ClientApplication>(solution.ClientApplication);

            clientApplication.Should().NotBeNull();

            clientApplication.MinimumConnectionSpeed.Should().Be(connectionSpeed);
            clientApplication.MinimumDesktopResolution.Should().Be(resolution);
        }

        public void Dispose()
        {
            ClearClientApplication(SolutionId);
        }
    }
}