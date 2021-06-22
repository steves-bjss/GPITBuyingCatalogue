﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.PublicBrowse.Solution
{
    public sealed class SolutionDetails : TestBase, IClassFixture<LocalWebApplicationFactory>
    {
        public SolutionDetails(LocalWebApplicationFactory factory) : base(factory, "solutions/futures/99999-001")
        {
        }

        [Theory]
        [InlineData("solution name")]
        [InlineData("solution id")]
        [InlineData("supplier name")]
        [InlineData("foundation solution")]
        [InlineData("framework")]
        public void SolutionDetails_AllFieldsDisplayed(string rowHeader)
        {
            PublicBrowsePages.SolutionAction.GetTableRowContent(rowHeader).Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task SolutionDetails_VerifySummary()
        {
            using var context = GetBCContext();
            var summary = (await context.Solutions.SingleAsync(s => s.Id == "99999-001")).Summary;

            var summaryAndDescription = PublicBrowsePages.SolutionAction.GetSummaryAndDescriptions();

            summaryAndDescription
                .Any(s => s.Contains(summary, StringComparison.CurrentCultureIgnoreCase))
                .Should().BeTrue();
        }

        [Fact]
        public async Task SolutionDetails_VerifyDescription()
        {
            using var context = GetBCContext();
            var description = (await context.Solutions.SingleAsync(s => s.Id == "99999-001")).FullDescription;

            var summaryAndDescription = PublicBrowsePages.SolutionAction.GetSummaryAndDescriptions();

            summaryAndDescription
                .Any(s => s.Contains(description, StringComparison.CurrentCultureIgnoreCase))
                .Should().BeTrue();
        }
    }
}