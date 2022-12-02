﻿using System;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using BuyingCatalogueFunction.Adapters;
using BuyingCatalogueFunction.Models.Ods;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.UnitTest.Framework.AutoFixtureCustomisations;
using Xunit;

namespace BuyingCatalogueFunctionTests.Adapters
{
    public static class OrganisationRelationshipsAdapterTests
    {
        [Fact]
        public static void Constructor_VerifyGuardClauses()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var assertion = new GuardClauseAssertion(fixture);
            var constructors = typeof(OrganisationRelationshipsAdapter).GetConstructors();

            assertion.Verify(constructors);
        }

        [Theory]
        [CommonAutoData]
        public static void Process_InputIsNull_ThrowsException(
            OrganisationRelationshipsAdapter adapter)
        {
            FluentActions
                .Invoking(() => adapter.Process(null))
                .Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [CommonAutoData]
        public static void Process_InputIsNotNull_ReturnsExpectedResult(
            Org input,
            OrganisationRelationshipsAdapter adapter)
        {
            var result = adapter.Process(input).ToList();

            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Count.Should().Be(input.Rels.Rel.Count);

            foreach (var output in result)
            {
                var existing = input.Rels.Rel.FirstOrDefault(x => x.uniqueRelId == output.Id);

                existing.Should().NotBeNull();

                output.OwnerOrganisationId.Should().Be(input.OrgId.extension);
                output.TargetOrganisationId.Should().Be(existing!.Target.OrgId.extension);
                output.RelationshipTypeId.Should().Be(existing.id);
            }
        }
    }
}