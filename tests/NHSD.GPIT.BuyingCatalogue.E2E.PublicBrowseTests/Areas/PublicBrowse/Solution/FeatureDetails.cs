﻿using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.PublicBrowse.Solution
{
    public sealed class FeatureDetails : TestBase, IClassFixture<LocalWebApplicationFactory>
    {
        public FeatureDetails(LocalWebApplicationFactory factory)
            : base(factory, "solutions/futures/99999-001/features")
        {
        }

        [Fact]
        public async Task FeatureDetails_VerifyFeatureContent()
        {
            {
                await using var context = GetEndToEndDbContext();
                var featureInfo = (await context.Solutions.SingleAsync(s => s.Id == new CatalogueItemId(99999, "001"))).Features;

                var featureList = PublicBrowsePages.SolutionAction.GetFeatureContent();

                var dbList = featureInfo.Replace("[", string.Empty).Replace("\"", string.Empty).Replace("]", string.Empty).Split(',').ToList().Select(s => s.Trim());
                featureList.Should().BeEquivalentTo(dbList);
            }
        }
    }
}
