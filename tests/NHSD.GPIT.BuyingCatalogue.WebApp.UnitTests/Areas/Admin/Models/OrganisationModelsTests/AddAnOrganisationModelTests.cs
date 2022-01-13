﻿using System.Collections.Generic;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Organisations.Models;
using NHSD.GPIT.BuyingCatalogue.Test.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.OrganisationModels;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Admin.Models.OrganisationModelsTests
{
    public static class AddAnOrganisationModelTests
    {
        [Theory]
        [CommonAutoData]
        public static void WithValidConstruction_PropertiesSetAsExpected(
            Organisation organisation,
            List<Organisation> availableOrganisations)
        {
            var actual = new AddAnOrganisationModel(organisation, availableOrganisations);

            actual.OrganisationId.Should().Be(organisation.Id);
            actual.OrganisationName.Should().Be(organisation.Name);
            actual.AvailableOrganisations.Should().BeEquivalentTo(availableOrganisations);
            actual.SelectedOrganisation.Should().Be(null);
        }
    }
}