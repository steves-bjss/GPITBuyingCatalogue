﻿using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Competitions.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.UnitTest.Framework.Attributes;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.EntityFramework.UnitTests.Models.Competitions;

public static class RequiredServiceTests
{
    [Theory]
    [MockAutoData]
    public static void Construct_SetsProperties(
        int competitionId,
        CatalogueItemId solutionId,
        CatalogueItemId serviceId)
    {
        var model = new SolutionService(competitionId, solutionId, serviceId, true);

        model.CompetitionId.Should().Be(competitionId);
        model.SolutionId.Should().Be(solutionId);
        model.ServiceId.Should().Be(serviceId);
        model.IsRequired.Should().BeTrue();
    }
}
