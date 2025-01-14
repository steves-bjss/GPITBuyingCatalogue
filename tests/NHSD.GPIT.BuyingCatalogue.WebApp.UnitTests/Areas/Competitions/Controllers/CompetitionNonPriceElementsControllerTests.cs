﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Competitions.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Extensions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Organisations.Models;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.Framework.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Competitions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Integrations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Competitions.Controllers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Competitions.Models.NonPriceElementModels;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Competitions.Models.NonPriceElementModels.FeaturesModels;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Competitions.Controllers;

public static class CompetitionNonPriceElementsControllerTests
{
    public static IEnumerable<object[]> GetRedirectNonPriceElementTestData => new[]
    {
        new object[]
        {
            new[]
            {
                NonPriceElement.Implementation,
                NonPriceElement.Interoperability,
                NonPriceElement.ServiceLevel,
            },
            NonPriceElement.Implementation,
        },
        new object[]
        {
            new[] { NonPriceElement.Interoperability, NonPriceElement.ServiceLevel },
            NonPriceElement.Interoperability,
        },
        new object[] { new[] { NonPriceElement.ServiceLevel }, NonPriceElement.ServiceLevel, },
    };

    [Fact]
    public static void Constructors_VerifyGuardClauses()
    {
        var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        var assertion = new GuardClauseAssertion(fixture);
        var constructors = typeof(CompetitionNonPriceElementsController).GetConstructors();

        assertion.Verify(constructors);
    }

    [Theory]
    [MockAutoData]
    public static async Task Index_ReturnsViewWithModel(
        Organisation organisation,
        Competition competition,
        List<Integration> integrations,
        [Frozen] ICompetitionsService competitionsService,
        [Frozen] IIntegrationsService integrationsService,
        CompetitionNonPriceElementsController controller)
    {
        competition.Organisation = organisation;

        competitionsService.GetCompetitionWithNonPriceElements(organisation.InternalIdentifier, competition.Id).Returns(competition);

        integrationsService.GetIntegrations().Returns(integrations);

        var expectedModel = new NonPriceElementsModel(competition, integrations);

        var result = (await controller.Index(organisation.InternalIdentifier, competition.Id)).As<ViewResult>();

        result.Should().NotBeNull();
        result.Model.Should().BeEquivalentTo(expectedModel, opt => opt.Excluding(m => m.BackLink));
    }

    [Theory]
    [MockAutoData]
    public static async Task AddNonPriceElement_ReturnsViewWithModel(
        Organisation organisation,
        Competition competition,
        [Frozen] ICompetitionsService competitionsService,
        CompetitionNonPriceElementsController controller)
    {
        competition.Organisation = organisation;

        competitionsService.GetCompetitionWithNonPriceElements(organisation.InternalIdentifier, competition.Id).Returns(competition);

        var expectedModel = new AddNonPriceElementModel(competition);

        var result = (await controller.AddNonPriceElement(organisation.InternalIdentifier, competition.Id))
            .As<ViewResult>();

        result.Should().NotBeNull();
        result.Model.Should().BeEquivalentTo(expectedModel, opt => opt.Excluding(m => m.BackLink));
    }

    [Theory]
    [MockAutoData]
    public static async Task AddNonPriceElement_NoAvailableNonPriceElements_Redirects(
        Organisation organisation,
        Competition competition,
        [Frozen] ICompetitionsService competitionsService,
        CompetitionNonPriceElementsController controller)
    {
        competition.Organisation = organisation;
        competition.NonPriceElements = new()
        {
            Implementation = new(),
            IntegrationTypes = new List<IntegrationType> { new(), },
            ServiceLevel = new(),
            Features = new List<FeaturesCriteria> { new() },
        };

        competitionsService.GetCompetitionWithNonPriceElements(organisation.InternalIdentifier, competition.Id).Returns(competition);

        var result = (await controller.AddNonPriceElement(organisation.InternalIdentifier, competition.Id))
            .As<RedirectToActionResult>();

        result.Should().NotBeNull();
        result.ActionName.Should().Be(nameof(controller.Index));
    }

    [Theory]
    [MockAutoData]
    public static void AddNonPriceElement_InvalidModel_ReturnsView(
        string internalOrgId,
        int competitionId,
        AddNonPriceElementModel model,
        CompetitionNonPriceElementsController controller)
    {
        controller.ModelState.AddModelError("some-key", "some-error");

        var result = controller.AddNonPriceElement(internalOrgId, competitionId, model).As<ViewResult>();

        result.Should().NotBeNull();
        result.Model.Should().Be(model);
    }

    [Theory]
    [MockAutoData]
    public static void AddNonPriceElement_SelectedElements_Redirects(
        string internalOrgId,
        int competitionId,
        AddNonPriceElementModel model,
        CompetitionNonPriceElementsController controller)
    {
        model.AvailableNonPriceElements = new List<SelectOption<NonPriceElement>>
        {
            new(NonPriceElement.Implementation.EnumMemberName(), NonPriceElement.Implementation, true),
            new(NonPriceElement.Interoperability.EnumMemberName(), NonPriceElement.Interoperability, true),
            new(NonPriceElement.ServiceLevel.EnumMemberName(), NonPriceElement.ServiceLevel, true),
        };

        var result = controller.AddNonPriceElement(internalOrgId, competitionId, model).As<RedirectToActionResult>();

        result.Should().NotBeNull();
        result.ActionName.Should().Be(NonPriceElement.Implementation.ToString());
        result.RouteValues.Should()
            .BeEquivalentTo(
                new RouteValueDictionary
                {
                    { nameof(internalOrgId), internalOrgId },
                    { nameof(competitionId), competitionId },
                    {
                        "selectedNonPriceElements",
                        string.Join(',', new[] { NonPriceElement.Interoperability, NonPriceElement.ServiceLevel })
                    },
                });
    }

    [Theory]
    [MockAutoData]
    public static async Task Interoperability_ReturnsViewWithModel(
        Organisation organisation,
        Competition competition,
        List<Integration> integrations,
        [Frozen] ICompetitionsService competitionsService,
        [Frozen] IIntegrationsService integrationsService,
        CompetitionNonPriceElementsController controller)
    {
        competition.Organisation = organisation;

        competitionsService.GetCompetitionWithNonPriceElements(organisation.InternalIdentifier, competition.Id)
            .Returns(competition);

        integrationsService.GetIntegrationsWithTypes().Returns(integrations);

        var expectedModel = new SelectInteroperabilityCriteriaModel(competition, integrations)
        {
            InternalOrgId = organisation.InternalIdentifier,
            CompetitionId = competition.Id,
        };

        var result =
            (await controller.Interoperability(organisation.InternalIdentifier, competition.Id)).As<ViewResult>();

        result.Should().NotBeNull();
        result.Model.Should().BeEquivalentTo(expectedModel, opt => opt.Excluding(m => m.BackLink));
    }

    [Theory]
    [MockAutoData]
    public static async Task Interoperability_InvalidModel_ReturnsView(
        string internalOrgId,
        int competitionId,
        SelectInteroperabilityCriteriaModel model,
        CompetitionNonPriceElementsController controller)
    {
        controller.ModelState.AddModelError("some-key", "some-error");

        var result = (await controller.Interoperability(internalOrgId, competitionId, model)).As<ViewResult>();

        result.Should().NotBeNull();
        result.Model.Should().Be(model);
    }

    [Theory]
    [MockAutoData]
    public static async Task Interoperability_Valid_SetsInteroperabilityCriteria(
        string internalOrgId,
        int competitionId,
        SelectInteroperabilityCriteriaModel model,
        [Frozen] ICompetitionsService competitionsService,
        CompetitionNonPriceElementsController controller)
    {
        var result =
            (await controller.Interoperability(internalOrgId, competitionId, model)).As<RedirectToActionResult>();

        await competitionsService.Received().SetInteroperabilityCriteria(
                internalOrgId,
                competitionId,
                Arg.Any<IEnumerable<int>>());

        result.Should().NotBeNull();
        result.ActionName.Should().Be(nameof(controller.Index));
    }

    [Theory]
    [MockAutoData]
    public static async Task Implementation_ReturnsViewWithModel(
        Organisation organisation,
        Competition competition,
        [Frozen] ICompetitionsService competitionsService,
        CompetitionNonPriceElementsController controller)
    {
        competition.Organisation = organisation;

        competitionsService.GetCompetitionWithNonPriceElements(organisation.InternalIdentifier, competition.Id).Returns(competition);

        var expectedModel = new AddImplementationCriteriaModel(competition)
        {
            InternalOrgId = organisation.InternalIdentifier,
            CompetitionId = competition.Id,
        };

        var result = (await controller.Implementation(organisation.InternalIdentifier, competition.Id))
            .As<ViewResult>();

        result.Should().NotBeNull();
        result.Model.Should().BeEquivalentTo(expectedModel, opt => opt.Excluding(m => m.BackLink));
    }

    [Theory]
    [MockAutoData]
    public static async Task Implementation_InvalidModel_ReturnsView(
        string internalOrgId,
        int competitionId,
        AddImplementationCriteriaModel model,
        CompetitionNonPriceElementsController controller)
    {
        controller.ModelState.AddModelError("some-key", "some-error");

        var result = (await controller.Implementation(internalOrgId, competitionId, model)).As<ViewResult>();

        result.Should().NotBeNull();
        result.Model.Should().Be(model);
    }

    [Theory]
    [MockAutoData]
    public static async Task Implementation_Valid_SetsImplementationCriteria(
        string internalOrgId,
        int competitionId,
        AddImplementationCriteriaModel model,
        [Frozen] ICompetitionsService competitionsService,
        CompetitionNonPriceElementsController controller)
    {
        var result = (await controller.Implementation(internalOrgId, competitionId, model))
            .As<RedirectToActionResult>();

        await competitionsService.Received().SetImplementationCriteria(internalOrgId, competitionId, model.Requirements);

        result.Should().NotBeNull();
        result.ActionName.Should().Be(nameof(controller.Index));
    }

    [Theory]
    [MockAutoData]
    public static async Task ServiceLevel_ReturnsViewWithModel(
        Organisation organisation,
        Competition competition,
        [Frozen] ICompetitionsService competitionsService,
        CompetitionNonPriceElementsController controller)
    {
        competition.Organisation = organisation;

        competitionsService.GetCompetitionWithNonPriceElements(organisation.InternalIdentifier, competition.Id).Returns(competition);

        var expectedModel = new AddServiceLevelCriteriaModel(competition)
        {
            InternalOrgId = organisation.InternalIdentifier,
            CompetitionId = competition.Id,
        };

        var result = (await controller.ServiceLevel(organisation.InternalIdentifier, competition.Id)).As<ViewResult>();

        result.Should().NotBeNull();
        result.Model.Should().BeEquivalentTo(expectedModel, opt => opt.Excluding(m => m.BackLink));
    }

    [Theory]
    [MockAutoData]
    public static async Task ServiceLevel_InvalidModel_ReturnsView(
        string internalOrgId,
        int competitionId,
        AddServiceLevelCriteriaModel model,
        CompetitionNonPriceElementsController controller)
    {
        controller.ModelState.AddModelError("some-key", "some-error");

        var result = (await controller.ServiceLevel(internalOrgId, competitionId, model)).As<ViewResult>();

        result.Should().NotBeNull();
        result.Model.Should().Be(model);
    }

    [Theory]
    [MockAutoData]
    public static async Task ServiceLevel_Valid_SetsServiceLevelCriteria(
        string internalOrgId,
        int competitionId,
        AddServiceLevelCriteriaModel model,
        [Frozen] ICompetitionsService competitionsService,
        CompetitionNonPriceElementsController controller)
    {
        var result = (await controller.ServiceLevel(internalOrgId, competitionId, model)).As<RedirectToActionResult>();

        await competitionsService.Received().SetServiceLevelCriteria(
                internalOrgId,
                competitionId,
                model.TimeFrom.GetValueOrDefault(),
                model.TimeUntil.GetValueOrDefault(),
                Arg.Any<IEnumerable<Iso8601DayOfWeek>>(),
                model.IncludesBankHolidays!.Value);

        result.Should().NotBeNull();
        result.ActionName.Should().Be(nameof(controller.Index));
    }

    [Theory]
    [MockAutoData]
    public static async Task Features_NoExistingFeaturesCriteria_RedirectsToFeatureRequirement(
        string internalOrgId,
        Competition competition,
        [Frozen] ICompetitionsService competitionsService,
        CompetitionNonPriceElementsController controller)
    {
        competition.NonPriceElements = new() { Features = new List<FeaturesCriteria>() };

        competitionsService.GetCompetitionWithNonPriceElements(internalOrgId, competition.Id).Returns(competition);

        var result = (await controller.Features(internalOrgId, competition.Id)).As<RedirectToActionResult>();

        result.Should().NotBeNull();
        result.ActionName.Should().Be(nameof(controller.FeatureRequirement));
    }

    [Theory]
    [MockAutoData]
    public static async Task Features_WithExistingFeaturesCriteria_ReturnsViewWithModel(
        string internalOrgId,
        Competition competition,
        List<FeaturesCriteria> featuresCriteria,
        [Frozen] ICompetitionsService competitionsService,
        CompetitionNonPriceElementsController controller)
    {
        competition.NonPriceElements = new() { Features = featuresCriteria };

        competitionsService.GetCompetitionWithNonPriceElements(internalOrgId, competition.Id).Returns(competition);

        var expectedModel = new FeaturesRequirementsModel(competition) { InternalOrgId = internalOrgId };

        var result = (await controller.Features(internalOrgId, competition.Id)).As<ViewResult>();

        result.Should().NotBeNull();
        result.Model.Should()
            .BeEquivalentTo(
                expectedModel,
                opt => opt.Excluding(m => m.BackLink)
                    .Excluding(m => m.SelectedNonPriceElements));
    }

    [Theory]
    [MockAutoData]
    public static async Task FeatureRequirement_ReturnsViewWithModel(
        string internalOrgId,
        Competition competition,
        [Frozen] ICompetitionsService competitionsService,
        CompetitionNonPriceElementsController controller)
    {
        competitionsService.GetCompetitionWithNonPriceElements(internalOrgId, competition.Id).Returns(competition);

        var expectedModel = new FeaturesRequirementModel(competition);

        var result = (await controller.FeatureRequirement(internalOrgId, competition.Id)).As<ViewResult>();

        result.Should().NotBeNull();
        result.Model.Should().BeEquivalentTo(expectedModel, opt => opt.Excluding(m => m.BackLink));
    }

    [Theory]
    [MockAutoData]
    public static async Task FeatureRequirement_InvalidModel_ReturnsViewWithModel(
        string internalOrgId,
        int competitionId,
        FeaturesRequirementModel model,
        CompetitionNonPriceElementsController controller)
    {
        controller.ModelState.AddModelError("some-key", "some-error");

        var result = (await controller.FeatureRequirement(internalOrgId, competitionId, model)).As<ViewResult>();

        result.Should().NotBeNull();
        result.Model.Should().Be(model);
    }

    [Theory]
    [MockAutoData]
    public static async Task FeatureRequirement_ValidModel_AddsFeatureRequirement(
        string internalOrgId,
        int competitionId,
        FeaturesRequirementModel model,
        [Frozen] ICompetitionNonPriceElementsService competitionNonPriceElementsService,
        CompetitionNonPriceElementsController controller)
    {
        _ = await controller.FeatureRequirement(internalOrgId, competitionId, model);

        await competitionNonPriceElementsService.Received()
            .AddFeatureRequirement(
                internalOrgId,
                competitionId,
                model.Requirements,
                model.SelectedCompliance!.Value);
    }

    [Theory]
    [MockAutoData]
    public static async Task FeatureRequirement_WithReturnUrl_ReturnsRedirectResult(
        string internalOrgId,
        int competitionId,
        string returnUrl,
        FeaturesRequirementModel model,
        CompetitionNonPriceElementsController controller)
    {
        var result = (await controller.FeatureRequirement(internalOrgId, competitionId, model, returnUrl))
            .As<RedirectResult>();

        result.Should().NotBeNull();
        result.Url.Should().Be(returnUrl);
    }

    [Theory]
    [MockAutoData]
    public static async Task FeatureRequirement_WithoutReturnUrl_ReturnsRedirectToActionResult(
        string internalOrgId,
        int competitionId,
        FeaturesRequirementModel model,
        CompetitionNonPriceElementsController controller)
    {
        var result = (await controller.FeatureRequirement(internalOrgId, competitionId, model))
            .As<RedirectToActionResult>();

        result.Should().NotBeNull();
        result.ActionName.Should().Be(nameof(controller.Features));
    }

    [Theory]
    [MockAutoData]
    public static async Task EditFeatureRequirement_InvalidRequirementId_ReturnsRedirect(
        string internalOrgId,
        Competition competition,
        int requirementId,
        [Frozen] ICompetitionsService competitionsService,
        CompetitionNonPriceElementsController controller)
    {
        competition.NonPriceElements = new();

        competitionsService.GetCompetitionWithNonPriceElements(internalOrgId, competition.Id).Returns(competition);

        var result = (await controller.EditFeatureRequirement(internalOrgId, competition.Id, requirementId))
            .As<RedirectToActionResult>();

        result.Should().NotBeNull();
        result.ActionName.Should().Be(nameof(controller.Index));
    }

    [Theory]
    [MockAutoData]
    public static async Task EditFeatureRequirement_ValidRequirementId_ReturnsViewWithModel(
        string internalOrgId,
        Competition competition,
        List<FeaturesCriteria> featuresCriteria,
        [Frozen] ICompetitionsService competitionsService,
        CompetitionNonPriceElementsController controller)
    {
        competition.NonPriceElements = new() { Features = featuresCriteria };

        competitionsService.GetCompetitionWithNonPriceElements(internalOrgId, competition.Id).Returns(competition);

        var requirement = featuresCriteria.First();

        var expectedModel = new FeaturesRequirementModel(competition, requirement)
        {
            InternalOrgId = internalOrgId,
            IsAdding = false,
        };

        var result = (await controller.EditFeatureRequirement(internalOrgId, competition.Id, requirement.Id))
            .As<ViewResult>();

        result.Should().NotBeNull();
        result.Model.Should().BeEquivalentTo(expectedModel, opt => opt.Excluding(m => m.BackLink));
    }

    [Theory]
    [MockAutoData]
    public static async Task EditFeatureRequirement_InvalidModel_AddsFeatureRequirement(
        string internalOrgId,
        int competitionId,
        int requirementId,
        FeaturesRequirementModel model,
        CompetitionNonPriceElementsController controller)
    {
        controller.ModelState.AddModelError("some-key", "some-error");

        var result = (await controller.EditFeatureRequirement(internalOrgId, competitionId, requirementId, model))
            .As<ViewResult>();

        result.Should().NotBeNull();
        result.Model.Should().Be(model);
    }

    [Theory]
    [MockAutoData]
    public static async Task EditFeatureRequirement_ValidModel_AddsFeatureRequirement(
        string internalOrgId,
        int competitionId,
        int requirementId,
        FeaturesRequirementModel model,
        [Frozen] ICompetitionNonPriceElementsService competitionNonPriceElementsService,
        CompetitionNonPriceElementsController controller)
    {
        _ = await controller.EditFeatureRequirement(internalOrgId, competitionId, requirementId, model);

        await competitionNonPriceElementsService.Received()
            .EditFeatureRequirement(
                internalOrgId,
                competitionId,
                requirementId,
                model.Requirements,
                model.SelectedCompliance!.Value);
    }

    [Theory]
    [MockAutoData]
    public static async Task EditFeatureRequirement_WithReturnUrl_ReturnsRedirectResult(
        string internalOrgId,
        int competitionId,
        int requirementId,
        string returnUrl,
        FeaturesRequirementModel model,
        CompetitionNonPriceElementsController controller)
    {
        var result = (await controller.EditFeatureRequirement(
                internalOrgId,
                competitionId,
                requirementId,
                model,
                returnUrl))
            .As<RedirectResult>();

        result.Should().NotBeNull();
        result.Url.Should().Be(returnUrl);
    }

    [Theory]
    [MockAutoData]
    public static async Task EditFeatureRequirement_WithoutReturnUrl_ReturnsRedirectToActionResult(
        string internalOrgId,
        int competitionId,
        int requirementId,
        FeaturesRequirementModel model,
        CompetitionNonPriceElementsController controller)
    {
        var result = (await controller.EditFeatureRequirement(internalOrgId, competitionId, requirementId, model))
            .As<RedirectToActionResult>();

        result.Should().NotBeNull();
        result.ActionName.Should().Be(nameof(controller.Features));
    }

    [Theory]
    [MockAutoData]
    public static async Task DeleteFeatureRequirement_IsAdding_MoreThanOneFeature_RedirectsToFeaturesReview(
        FeaturesCriteria firstCriteria,
        FeaturesCriteria secondCriteria,
        string internalOrgId,
        Competition competition,
        [Frozen] ICompetitionsService competitionsService,
        CompetitionNonPriceElementsController controller)
    {
        competition.NonPriceElements = new()
        {
            Features = new List<FeaturesCriteria> { firstCriteria, secondCriteria, },
        };

        competitionsService.GetCompetitionWithNonPriceElements(internalOrgId, competition.Id).Returns(competition);

        var result = (await controller.DeleteFeatureRequirement(
                internalOrgId,
                competition.Id,
                firstCriteria.Id,
                isAdding: true))
            .As<RedirectToActionResult>();

        result.Should().NotBeNull();
        result.ActionName.Should().Be(nameof(controller.Features));
    }

    [Theory]
    [MockAutoData]
    public static async Task DeleteFeatureRequirement_IsAdding_SingleFeature_Redirects(
        FeaturesCriteria firstCriteria,
        string internalOrgId,
        Competition competition,
        [Frozen] ICompetitionsService competitionsService,
        CompetitionNonPriceElementsController controller)
    {
        competition.NonPriceElements = new()
        {
            Features = new List<FeaturesCriteria> { firstCriteria, },
        };

        competitionsService.GetCompetitionWithNonPriceElements(internalOrgId, competition.Id).Returns(competition);

        var result = (await controller.DeleteFeatureRequirement(
                internalOrgId,
                competition.Id,
                firstCriteria.Id,
                isAdding: true))
            .As<RedirectToActionResult>();

        result.Should().NotBeNull();
        result.ActionName.Should().Be(nameof(controller.Index));
    }

    [Theory]
    [MockAutoData]
    public static async Task DeleteFeatureRequirement_IsNotAdding_MultipleFeatures_Redirects(
        FeaturesCriteria firstCriteria,
        FeaturesCriteria secondCriteria,
        string internalOrgId,
        Competition competition,
        [Frozen] ICompetitionsService competitionsService,
        CompetitionNonPriceElementsController controller)
    {
        competition.NonPriceElements = new()
        {
            Features = new List<FeaturesCriteria> { firstCriteria, secondCriteria, },
        };

        competitionsService.GetCompetitionWithNonPriceElements(internalOrgId, competition.Id).Returns(competition);

        var result = (await controller.DeleteFeatureRequirement(
                internalOrgId,
                competition.Id,
                firstCriteria.Id,
                isAdding: false))
            .As<RedirectToActionResult>();

        result.Should().NotBeNull();
        result.ActionName.Should().Be(nameof(controller.Index));
    }

    [Theory]
    [MockAutoData]
    public static async Task DeleteFeatureRequirement_IsNotAdding_SingleFeature_ReturnsViewWithModel(
        FeaturesCriteria firstCriteria,
        string internalOrgId,
        Competition competition,
        [Frozen] ICompetitionsService competitionsService,
        CompetitionNonPriceElementsController controller)
    {
        competition.NonPriceElements = new()
        {
            Features = new List<FeaturesCriteria> { firstCriteria, },
        };

        competitionsService.GetCompetitionWithNonPriceElements(internalOrgId, competition.Id).Returns(competition);

        var expectedModel = new DeleteNonPriceElementModel(NonPriceElement.Features);

        var result = (await controller.DeleteFeatureRequirement(
                internalOrgId,
                competition.Id,
                firstCriteria.Id,
                isAdding: false))
            .As<ViewResult>();

        result.Should().NotBeNull();
        result.Model.Should().BeEquivalentTo(expectedModel, opt => opt.Excluding(m => m.BackLink));
    }

    [Theory]
    [MockAutoData]
    public static async Task Post_DeleteFeatureRequirement_Redirects(
        string internalOrgId,
        int competitionId,
        int requirementId,
        DeleteNonPriceElementModel model,
        CompetitionNonPriceElementsController controller)
    {
        var result = (await controller.DeleteFeatureRequirement(internalOrgId, competitionId, requirementId, model))
            .As<RedirectToActionResult>();

        result.Should().NotBeNull();
        result.ActionName.Should().Be(nameof(controller.Index));
    }

    [Theory]
    [MockAutoData]
    public static async Task Weights_ReturnsViewWithModel(
        string internalOrgId,
        Competition competition,
        [Frozen] ICompetitionsService competitionsService,
        CompetitionNonPriceElementsController controller)
    {
        competitionsService.GetCompetitionWithNonPriceElements(internalOrgId, competition.Id).Returns(competition);

        var expectedModel = new NonPriceElementWeightsModel(competition);

        var result = (await controller.Weights(internalOrgId, competition.Id)).As<ViewResult>();

        result.Should().NotBeNull();
        result.Model.Should().BeEquivalentTo(expectedModel, opt => opt.Excluding(m => m.BackLink));
    }

    [Theory]
    [MockAutoData]
    public static async Task Weights_InvalidModel_ReturnsViewWithModel(
        string internalOrgId,
        int competitionId,
        NonPriceElementWeightsModel model,
        CompetitionNonPriceElementsController controller)
    {
        controller.ModelState.AddModelError("some-key", "some-error");

        var result = (await controller.Weights(internalOrgId, competitionId, model)).As<ViewResult>();

        result.Should().NotBeNull();
        result.Model.Should().Be(model);
    }

    [Theory]
    [MockAutoData]
    public static async Task Weights_Valid_SetsNonPriceWeights(
        string internalOrgId,
        int competitionId,
        NonPriceElementWeightsModel model,
        [Frozen] ICompetitionsService competitionsService,
        CompetitionNonPriceElementsController controller)
    {
        var result = (await controller.Weights(internalOrgId, competitionId, model)).As<RedirectToActionResult>();

        await competitionsService.Received().SetNonPriceWeights(
                internalOrgId,
                competitionId,
                model.Implementation.GetValueOrDefault(),
                model.Interoperability.GetValueOrDefault(),
                model.ServiceLevel.GetValueOrDefault(),
                model.Features.GetValueOrDefault());

        result.Should().NotBeNull();
        result.ActionName.Should().Be(nameof(CompetitionTaskListController.Index));
        result.ControllerName.Should().Be(typeof(CompetitionTaskListController).ControllerName());
    }

    [Theory]
    [MockAutoData]
    public static void Delete_ReturnsViewWithModel(
        string internalOrgId,
        int competitionId,
        NonPriceElement nonPriceElement,
        CompetitionNonPriceElementsController controller)
    {
        var expectedModel = new DeleteNonPriceElementModel(nonPriceElement);

        var result = controller.Delete(internalOrgId, competitionId, nonPriceElement).As<ViewResult>();

        result.Should().NotBeNull();
        result.Model.Should().BeEquivalentTo(expectedModel, opt => opt.Excluding(m => m.BackLink));
    }

    [Theory]
    [MockAutoData]
    public static async Task Delete_Redirects(
        string internalOrgId,
        int competitionId,
        NonPriceElement nonPriceElement,
        DeleteNonPriceElementModel model,
        CompetitionNonPriceElementsController controller)
    {
        var result = (await controller.Delete(internalOrgId, competitionId, nonPriceElement, model))
            .As<RedirectToActionResult>();

        result.Should().NotBeNull();
        result.ActionName.Should().Be(nameof(controller.Index));
    }

    [Theory]
    [MockAutoData]
    public static void GetRedirect_WithReturnUrl_ReturnsRedirect(
        string internalOrgId,
        int competitionId,
        string returnUrl,
        CompetitionNonPriceElementsController controller)
    {
        var result = controller.GetRedirect(internalOrgId, competitionId, returnUrl, null).As<RedirectResult>();

        result.Should().NotBeNull();
        result.Url.Should().Be(returnUrl);
    }

    [Theory]
    [MockAutoData]
    public static void GetRedirect_WithNoSelectedNonPriceElements_RedirectsToIndex(
        string internalOrgId,
        int competitionId,
        CompetitionNonPriceElementsController controller)
    {
        const string returnUrl = null;
        const string selectedNonPriceElements = null;

        var result = controller.GetRedirect(internalOrgId, competitionId, returnUrl, selectedNonPriceElements)
            .As<RedirectToActionResult>();

        result.Should().NotBeNull();
        result.ActionName.Should().Be(nameof(controller.Index));
    }

    [Theory]
    [MockMemberAutoData(nameof(GetRedirectNonPriceElementTestData))]
    public static void GetRedirect_WithSelectedNonPriceElements_RedirectsToNonPriceElement(
        NonPriceElement[] nonPriceElements,
        NonPriceElement expectedNonPriceElementAction,
        string internalOrgId,
        int competitionId,
        CompetitionNonPriceElementsController controller)
    {
        const string returnUrl = null;

        var selectedNonPriceElements = string.Join(',', nonPriceElements);

        var result = controller.GetRedirect(internalOrgId, competitionId, returnUrl, selectedNonPriceElements)
            .As<RedirectToActionResult>();

        result.Should().NotBeNull();
        result.ActionName.Should().Be(expectedNonPriceElementAction.ToString());
    }
}
