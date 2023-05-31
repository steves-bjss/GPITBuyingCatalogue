﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Models.FilterModels;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;
using NHSD.GPIT.BuyingCatalogue.UnitTest.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Competitions.Models.DashboardModels;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Competitions.Models;

public static class ReviewFilterModelTests
{
    public static IEnumerable<object[]> HasEpicsTestData => new[]
    {
        new object[]
        {
            new List<KeyValuePair<string, List<string>>> { new("Capability", new List<string> { "Epic" }), },
            true,
        },
        new object[] { Enumerable.Empty<KeyValuePair<string, List<string>>>().ToList(), false },
    };

    public static IEnumerable<object[]> HasHostingTypesTestData => new[]
    {
        new object[] { new List<HostingType> { HostingType.Hybrid }, true, },
        new object[] { Enumerable.Empty<HostingType>().ToList(), false, },
    };

    public static IEnumerable<object[]> HasClientApplicationTypesTestData => new[]
    {
        new object[] { new List<ClientApplicationType> { ClientApplicationType.Desktop }, true, },
        new object[] { Enumerable.Empty<ClientApplicationType>().ToList(), false, },
    };

    public static IEnumerable<object[]> HasAdditionalFiltersTestData => new[]
    {
        new object[] { new FilterDetailsModel(), false },
        new object[] { new FilterDetailsModel { FrameworkName = "Framework" }, true },
        new object[]
        {
            new FilterDetailsModel
            {
                ClientApplicationTypes = new List<ClientApplicationType> { ClientApplicationType.Desktop },
            },
            true,
        },
        new object[]
        {
            new FilterDetailsModel { HostingTypes = new List<HostingType> { HostingType.Hybrid }, }, true,
        },
        new object[]
        {
            new FilterDetailsModel
            {
                FrameworkName = "Framework",
                ClientApplicationTypes = new List<ClientApplicationType> { ClientApplicationType.Desktop },
                HostingTypes = new List<HostingType> { HostingType.Hybrid },
            },
            true,
        },
    };

    [Theory]
    [CommonAutoData]
    public static void Construct_SetsProperties(
        FilterDetailsModel filterDetailsModel)
    {
        var model = new ReviewFilterModel(filterDetailsModel);

        model.FilterDetails.Should().Be(filterDetailsModel);
    }

    [Theory]
    [CommonInlineAutoData("Framework", true)]
    [CommonInlineAutoData(null, false)]
    public static void HasFramework_ReturnsExpected(
        string framework,
        bool expected,
        FilterDetailsModel filterDetailsModel)
    {
        filterDetailsModel.FrameworkName = framework;

        var model = new ReviewFilterModel(filterDetailsModel);

        model.HasFramework().Should().Be(expected);
    }

    [Theory]
    [CommonMemberAutoData(nameof(HasEpicsTestData))]
    public static void HasEpics_ReturnsExpected(
        List<KeyValuePair<string, List<string>>> capabilities,
        bool expected,
        FilterDetailsModel filterDetailsModel)
    {
        filterDetailsModel.Capabilities = capabilities;

        var model = new ReviewFilterModel(filterDetailsModel);

        model.HasEpics().Should().Be(expected);
    }

    [Theory]
    [CommonMemberAutoData(nameof(HasHostingTypesTestData))]
    public static void HasHostingTypes_ReturnsExpected(
        List<HostingType> hostingTypes,
        bool expected,
        FilterDetailsModel filterDetailsModel)
    {
        filterDetailsModel.HostingTypes = hostingTypes;

        var model = new ReviewFilterModel(filterDetailsModel);

        model.HasHostingTypes().Should().Be(expected);
    }

    [Theory]
    [CommonMemberAutoData(nameof(HasClientApplicationTypesTestData))]
    public static void HasClientApplicationTypes_ReturnsExpected(
        List<ClientApplicationType> clientApplicationTypes,
        bool expected,
        FilterDetailsModel filterDetailsModel)
    {
        filterDetailsModel.ClientApplicationTypes = clientApplicationTypes;

        var model = new ReviewFilterModel(filterDetailsModel);

        model.HasClientApplicationTypes().Should().Be(expected);
    }

    [Theory]
    [CommonMemberAutoData(nameof(HasAdditionalFiltersTestData))]
    public static void HasAdditionalFilters_ReturnsExpected(
        FilterDetailsModel filterDetailsModel,
        bool expected)
    {
        var model = new ReviewFilterModel(filterDetailsModel);

        model.HasAdditionalFilters().Should().Be(expected);
    }
}