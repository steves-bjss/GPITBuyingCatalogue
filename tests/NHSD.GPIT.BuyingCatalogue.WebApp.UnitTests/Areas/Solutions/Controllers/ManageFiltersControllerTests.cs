﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Filtering.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Organisations.Models;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Capabilities;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Frameworks;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Integrations;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Models.FilterModels;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Organisations;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Pdf;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Orders.Controllers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Solutions.Controllers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Solutions.Models.Filters;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Solutions.Models.ManageFilters;
using NHSD.GPIT.BuyingCatalogue.WebApp.Models;
using NHSD.GPIT.BuyingCatalogue.WebApp.Models.Shared;
using Xunit;
using ApplicationType = NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models.ApplicationType;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Solutions.Controllers
{
    public static class ManageFiltersControllerTests
    {
        [Fact]
        public static void Constructors_VerifyGuardClauses()
        {
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            var assertion = new GuardClauseAssertion(fixture);
            var constructors = typeof(ManageFiltersController).GetConstructors();

            assertion.Verify(constructors);
        }

        [Theory]
        [MockAutoData]
        public static async Task Get_Index_ReturnsExpectedResult(
            [Frozen] IOrganisationsService organisationsService,
            [Frozen] ICapabilitiesService capabilitiesService,
            [Frozen] IFrameworkService frameworkService,
            [Frozen] IManageFiltersService manageFiltersService,
            [Frozen] ISolutionsFilterService solutionsFilterService,
            [Frozen] IUrlHelper mockUrlHelper,
            [Frozen] IPdfService mockPdfService,
            [Frozen] IIntegrationsService integrationsService,
            string primaryOrganisationInternalId,
            Organisation organisation,
            List<Filter> existingFilters)
        {
            organisationsService.GetOrganisationByInternalIdentifier(primaryOrganisationInternalId)
                .Returns(Task.FromResult(organisation));

            manageFiltersService.GetFilters(organisation.Id).Returns(Task.FromResult(existingFilters));

            var controller = CreateController(
                organisationsService,
                capabilitiesService,
                frameworkService,
                manageFiltersService,
                solutionsFilterService,
                mockPdfService,
                integrationsService,
                primaryOrganisationInternalId);

            controller.Url = mockUrlHelper;

            var result = await controller.Index();

            var actualResult = result.Should().BeOfType<ViewResult>().Subject;

            var expected = new ManageFiltersModel(organisation.InternalIdentifier, existingFilters, organisation.Name);
            actualResult.Model.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [MockAutoData]
        public static void Post_SaveFilter_ReturnsExpectedResult(
            AdditionalFiltersModel model,
            ManageFiltersController controller)
        {
            var result = controller.SaveFilter(model);

            var actualResult = result.Should().BeOfType<RedirectToActionResult>().Subject;

            actualResult.ActionName.Should().Be(nameof(ManageFiltersController.ConfirmSaveFilter));
            actualResult.ControllerName.Should().Be(typeof(ManageFiltersController).ControllerName());
            actualResult.RouteValues.Should()
                .BeEquivalentTo(
                    new RouteValueDictionary
                    {
                        { "selected", model.Selected },
                        { "selectedFrameworkId", model.SelectedFrameworkId },
                        { "selectedApplicationTypeIds", model.CombineSelectedOptions(model.ApplicationTypeOptions) },
                        { "selectedHostingTypeIds", model.CombineSelectedOptions(model.HostingTypeOptions) },
                        { "selectedIntegrations", model.GetIntegrationIds() },
                    });
        }

        [Theory]
        [MockAutoData]
        public static async Task Get_DownloadResults_ValidFilter_ReturnsExpectedResult(
            FilterDetailsModel filter,
            [Frozen] IOrganisationsService organisationsService,
            [Frozen] ICapabilitiesService capabilitiesService,
            [Frozen] IFrameworkService frameworkService,
            [Frozen] IManageFiltersService manageFiltersService,
            [Frozen] ISolutionsFilterService solutionsFilterService,
            [Frozen] IUrlHelper mockUrlHelper,
            [Frozen] IPdfService mockPdfService,
            [Frozen] IIntegrationsService integrationsService,
            byte[] fileContents,
            string primaryOrganisationInternalId,
            Organisation organisation)
        {
            organisationsService.GetOrganisationByInternalIdentifier(primaryOrganisationInternalId)
                .Returns(Task.FromResult(organisation));

            manageFiltersService.GetFilterDetails(organisation.Id, filter.Id).Returns(Task.FromResult(filter));

            mockPdfService.Convert(Arg.Any<Uri>()).Returns(Task.FromResult(fileContents));

            mockPdfService.BaseUri().Returns(new Uri("http://localhost"));

            var controller = CreateController(
                organisationsService,
                capabilitiesService,
                frameworkService,
                manageFiltersService,
                solutionsFilterService,
                mockPdfService,
                integrationsService,
                primaryOrganisationInternalId);

            controller.Url = mockUrlHelper;

            var result = (await controller.DownloadResults(filter.Id)).As<FileContentResult>();

            result.Should().NotBeNull();
            result.ContentType.Should().Be("application/pdf");
            result.FileDownloadName.Should().Be($"{filter.Name} Catalogue Solutions.pdf");
            result.FileContents.Should().BeEquivalentTo(fileContents);
        }

        [Theory]
        [MockAutoData]
        public static async Task Get_DownloadResults_NullFilter_ReturnsExpectedResult(
            FilterDetailsModel filter,
            [Frozen] IOrganisationsService organisationsService,
            [Frozen] ICapabilitiesService capabilitiesService,
            [Frozen] IFrameworkService frameworkService,
            [Frozen] IManageFiltersService manageFiltersService,
            [Frozen] ISolutionsFilterService solutionsFilterService,
            [Frozen] IUrlHelper mockUrlHelper,
            [Frozen] IPdfService mockPdfService,
            [Frozen] IIntegrationsService integrationsService,
            string primaryOrganisationInternalId,
            Organisation organisation)
        {
            organisationsService.GetOrganisationByInternalIdentifier(primaryOrganisationInternalId)
                .Returns(Task.FromResult(organisation));

            manageFiltersService.GetFilterDetails(organisation.Id, filter.Id)
                .Returns(Task.FromResult((FilterDetailsModel)null));

            var controller = CreateController(
                organisationsService,
                capabilitiesService,
                frameworkService,
                manageFiltersService,
                solutionsFilterService,
                mockPdfService,
                integrationsService,
                primaryOrganisationInternalId);

            controller.Url = mockUrlHelper;

            var result = await controller.DownloadResults(filter.Id);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Theory]
        [MockAutoData]
        public static async Task Get_FilterDetails_NullFilter_ReturnsNotFound(
            Filter filter,
            [Frozen] IOrganisationsService organisationsService,
            [Frozen] ICapabilitiesService capabilitiesService,
            [Frozen] IFrameworkService frameworkService,
            [Frozen] IManageFiltersService manageFiltersService,
            [Frozen] ISolutionsFilterService solutionsFilterService,
            [Frozen] IUrlHelper mockUrlHelper,
            [Frozen] IPdfService mockPdfService,
            [Frozen] IIntegrationsService integrationsService,
            string primaryOrganisationInternalId,
            Organisation organisation)
        {
            organisationsService.GetOrganisationByInternalIdentifier(primaryOrganisationInternalId)
                .Returns(Task.FromResult(organisation));

            manageFiltersService.GetFilterDetails(organisation.Id, filter.Id)
                .Returns(Task.FromResult((FilterDetailsModel)null));

            var controller = CreateController(
                organisationsService,
                capabilitiesService,
                frameworkService,
                manageFiltersService,
                solutionsFilterService,
                mockPdfService,
                integrationsService,
                primaryOrganisationInternalId);

            controller.Url = mockUrlHelper;

            var result = await controller.FilterDetails(filter.Id);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Theory]
        [MockAutoData]
        public static async Task Get_FilterDetails_ReturnsExpectedResult(
            string primaryOrganisationInternalId,
            int filterId,
            FilterDetailsModel filterDetailsModel,
            List<CatalogueItem> filterResults,
            Solution solution,
            FilterIdsModel filterIds,
            Organisation organisation,
            [Frozen] IOrganisationsService organisationsService,
            [Frozen] ICapabilitiesService capabilitiesService,
            [Frozen] IFrameworkService frameworkService,
            [Frozen] IManageFiltersService manageFiltersService,
            [Frozen] ISolutionsFilterService solutionsFilterService,
            [Frozen] IUrlHelper mockUrlHelper,
            [Frozen] IPdfService mockPdfService,
            [Frozen] IIntegrationsService integrationsService)
        {
            organisationsService.GetOrganisationByInternalIdentifier(primaryOrganisationInternalId)
                .Returns(Task.FromResult(organisation));

            manageFiltersService.GetFilterDetails(organisation.Id, filterId)
                .Returns(Task.FromResult(filterDetailsModel));

            manageFiltersService.GetFilterIds(organisation.Id, filterId).Returns(Task.FromResult(filterIds));

            filterResults.ForEach(x => x.Solution = solution);

            solutionsFilterService.GetAllSolutionsFilteredFromFilterIds(filterIds).Returns(filterResults);

            var controller = CreateController(
                organisationsService,
                capabilitiesService,
                frameworkService,
                manageFiltersService,
                solutionsFilterService,
                mockPdfService,
                integrationsService,
                primaryOrganisationInternalId);

            controller.Url = mockUrlHelper;

            var result = await controller.FilterDetails(filterId);

            var actualResult = result.Should().BeOfType<ViewResult>().Subject;

            var expected =
                new ReviewFilterModel(
                    filterDetailsModel,
                    organisation.InternalIdentifier,
                    filterResults,
                    false,
                    filterIds)
                {
                    Caption = organisation.Name,
                    OrganisationName = organisation.Name,
                    InExpander = true,
                };
            actualResult.Model.Should().BeEquivalentTo(expected, opt => opt.Excluding(x => x.BackLink));
        }

        [Theory]
        [MockAutoData]
        public static void Get_CannotSaveFilter_ReturnsExpectedResult(
            ManageFiltersController controller)
        {
            var result = controller.CannotSaveFilter(string.Empty);

            var actualResult = result.Should().BeOfType<ViewResult>().Subject;
            actualResult.Model.Should().BeOfType<NavBaseModel>();
        }

        [Theory]
        [MockInlineAutoData(10)]
        [MockInlineAutoData(11)]
        public static async Task Get_ConfirmSaveFilter_TooManyFilters_ReturnsExpectedResult(
            int numberOfExistingFilters,
            [Frozen] IOrganisationsService organisationsService,
            [Frozen] ICapabilitiesService capabilitiesService,
            [Frozen] IFrameworkService frameworkService,
            [Frozen] IManageFiltersService manageFiltersService,
            [Frozen] ISolutionsFilterService solutionsFilterService,
            [Frozen] IUrlHelper mockUrlHelper,
            [Frozen] IPdfService mockPdfService,
            [Frozen] IIntegrationsService integrationsService,
            string primaryOrganisationInternalId,
            Organisation organisation)
        {
            var existingFilters = new List<Filter>(new Filter[numberOfExistingFilters]);

            organisationsService.GetOrganisationByInternalIdentifier(primaryOrganisationInternalId)
                .Returns(Task.FromResult(organisation));

            manageFiltersService.GetFilters(organisation.Id).Returns(Task.FromResult(existingFilters));

            var controller = CreateController(
                organisationsService,
                capabilitiesService,
                frameworkService,
                manageFiltersService,
                solutionsFilterService,
                mockPdfService,
                integrationsService,
                primaryOrganisationInternalId);

            controller.Url = mockUrlHelper;

            var result = await controller.ConfirmSaveFilter(
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty);

            await organisationsService.Received().GetOrganisationByInternalIdentifier(primaryOrganisationInternalId);
            await manageFiltersService.Received().GetFilters(organisation.Id);

            var actualResult = result.Should().BeOfType<RedirectToActionResult>().Subject;

            actualResult.ActionName.Should().Be(nameof(ManageFiltersController.CannotSaveFilter));
            actualResult.ControllerName.Should().Be(typeof(ManageFiltersController).ControllerName());
        }

        [Theory]
        [MockAutoData]
        public static async Task Get_ConfirmSaveFilter_ReturnsExpectedResult(
            [Frozen] IOrganisationsService organisationsService,
            [Frozen] ICapabilitiesService capabilitiesService,
            [Frozen] IFrameworkService frameworkService,
            [Frozen] IManageFiltersService manageFiltersService,
            [Frozen] ISolutionsFilterService solutionsFilterService,
            [Frozen] IUrlHelper mockUrlHelper,
            [Frozen] IPdfService mockPdfService,
            [Frozen] IIntegrationsService integrationsService,
            string primaryOrganisationInternalId,
            Organisation organisation,
            Dictionary<int, string[]> capabilityAndEpics,
            Dictionary<SupportedIntegrations, int[]> selectedIntegrations,
            Dictionary<string, IOrderedEnumerable<Epic>> groupedEpics,
            Dictionary<string, IOrderedEnumerable<string>> groupedIntegrations,
            string selectedFrameworkId)
        {
            var existingFilters = new List<Filter>();

            organisationsService.GetOrganisationByInternalIdentifier(primaryOrganisationInternalId)
                .Returns(organisation);

            manageFiltersService.GetFilters(organisation.Id).Returns(existingFilters);

            capabilitiesService.GetGroupedCapabilitiesAndEpics(Arg.Any<Dictionary<int, string[]>>())
                .Returns(groupedEpics);

            frameworkService.GetFramework(selectedFrameworkId)
                .Returns((EntityFramework.Catalogue.Models.Framework)null);

            integrationsService.GetIntegrationAndTypeNames(Arg.Any<Dictionary<SupportedIntegrations, int[]>>())
                .Returns(groupedIntegrations);

            var controller = CreateController(
                organisationsService,
                capabilitiesService,
                frameworkService,
                manageFiltersService,
                solutionsFilterService,
                mockPdfService,
                integrationsService,
                primaryOrganisationInternalId);

            controller.Url = mockUrlHelper;

            var result = await controller.ConfirmSaveFilter(
                capabilityAndEpics.ToFilterString(),
                selectedFrameworkId,
                string.Empty,
                string.Empty,
                selectedIntegrations.ToFilterString());

            await organisationsService.Received().GetOrganisationByInternalIdentifier(primaryOrganisationInternalId);
            await manageFiltersService.Received().GetFilters(organisation.Id);
            await capabilitiesService.Received().GetGroupedCapabilitiesAndEpics(Arg.Any<Dictionary<int, string[]>>());
            await frameworkService.Received().GetFramework(selectedFrameworkId);

            var actualResult = result.Should().BeOfType<ViewResult>().Subject;
            var actualResultModel = actualResult.Model.Should().BeOfType<SaveFilterModel>().Subject;

            var expected = new SaveFilterModel(
                groupedEpics,
                null,
                new List<ApplicationType>(),
                new List<HostingType>(),
                groupedIntegrations,
                organisation.Id);

            actualResultModel.Should()
                .BeEquivalentTo(expected, opt => opt.Excluding(m => m.BackLink).Excluding(m => m.BackLinkText));
        }

        [Theory]
        [MockAutoData]
        public static async Task Post_ConfirmSaveFilter_WithModelErrors_ReturnsExpectedResult(
            SaveFilterModel model,
            Dictionary<string, IOrderedEnumerable<Epic>> groupedEpics,
            Dictionary<int, string[]> capabilityAndEpics,
            Dictionary<SupportedIntegrations, int[]> integrations,
            [Frozen] ICapabilitiesService capabilitiesService,
            [Frozen] IEpicsService epicsService,
            ManageFiltersController controller)
        {
            controller.ModelState.AddModelError("key", "errorMessage");

            capabilitiesService.GetGroupedCapabilitiesAndEpics(Arg.Any<Dictionary<int, string[]>>())
                .Returns(Task.FromResult(groupedEpics));

            var result = await controller.ConfirmSaveFilter(
                model,
                capabilityAndEpics.ToFilterString(),
                integrations.ToFilterString());

            await capabilitiesService.Received().GetGroupedCapabilitiesAndEpics(Arg.Any<Dictionary<int, string[]>>());
            epicsService.ReceivedCalls().Should().BeEmpty();

            var actualResult = result.Should().BeOfType<ViewResult>().Subject;
            var resultModel = actualResult.Model.Should().BeOfType<SaveFilterModel>().Subject;
            resultModel.GroupedCapabilities.Count.Should().Be(groupedEpics.Count);
        }

        [Theory]
        [MockAutoData]
        public static async Task Post_ConfirmSaveFilter_ReturnsExpectedResult(
            SaveFilterModel model,
            Dictionary<int, string[]> capabilityAndEpics,
            Dictionary<SupportedIntegrations, int[]> integrations,
            int filterId,
            [Frozen] IManageFiltersService manageFiltersService,
            ManageFiltersController controller)
        {
            manageFiltersService.AddFilter(
                    model.Name,
                    model.Description,
                    model.OrganisationId,
                    Arg.Is<Dictionary<int, string[]>>(x => x.ToFilterString() == capabilityAndEpics.ToFilterString()),
                    model.FrameworkId,
                    model.ApplicationTypes,
                    model.HostingTypes,
                    Arg.Is<Dictionary<SupportedIntegrations, int[]>>(
                        x => x.ToFilterString() == integrations.ToFilterString()))
                .Returns(Task.FromResult(filterId));

            var result = await controller.ConfirmSaveFilter(
                model,
                capabilityAndEpics.ToFilterString(),
                integrations.ToFilterString());

            await manageFiltersService.Received()
                .AddFilter(
                    model.Name,
                    model.Description,
                    model.OrganisationId,
                    Arg.Is<Dictionary<int, string[]>>(x => x.ToFilterString() == capabilityAndEpics.ToFilterString()),
                    model.FrameworkId,
                    model.ApplicationTypes,
                    model.HostingTypes,
                    Arg.Is<Dictionary<SupportedIntegrations, int[]>>(
                        x => x.ToFilterString() == integrations.ToFilterString()));

            var actualResult = result.Should().BeOfType<RedirectToActionResult>().Subject;

            actualResult.ActionName.Should().Be(nameof(ManageFiltersController.FilterDetails));
            actualResult.RouteValues.Should()
                .BeEquivalentTo(new RouteValueDictionary { { nameof(filterId), filterId }, });
        }

        [Theory]
        [MockAutoData]
        public static async Task Get_DeleteFilter_ReturnsViewResult(
            string primaryOrganisationInternalId,
            int filterId,
            FilterDetailsModel filterDetailsModel,
            Organisation organisation,
            [Frozen] IOrganisationsService organisationsService,
            [Frozen] ICapabilitiesService capabilitiesService,
            [Frozen] IFrameworkService frameworkService,
            [Frozen] IManageFiltersService manageFiltersService,
            [Frozen] ISolutionsFilterService solutionsFilterService,
            [Frozen] IUrlHelper mockUrlHelper,
            [Frozen] IPdfService mockPdfService,
            [Frozen] IIntegrationsService integrationsService)
        {
            organisationsService.GetOrganisationByInternalIdentifier(primaryOrganisationInternalId)
                .Returns(Task.FromResult(organisation));
            manageFiltersService.GetFilterDetails(organisation.Id, filterId)
                .Returns(Task.FromResult(filterDetailsModel));
            var controller = CreateController(
                organisationsService,
                capabilitiesService,
                frameworkService,
                manageFiltersService,
                solutionsFilterService,
                mockPdfService,
                integrationsService,
                primaryOrganisationInternalId);
            controller.Url = mockUrlHelper;

            var result = await controller.DeleteFilter(filterId);

            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeOfType<DeleteFilterModel>();
        }

        [Theory]
        [MockAutoData]
        public static async Task Get_DeleteFilter_NullFilter_ReturnsRedirectToActionResult(
            string primaryOrganisationInternalId,
            int filterId,
            FilterDetailsModel filterDetailsModel,
            Organisation organisation,
            [Frozen] IOrganisationsService organisationsService,
            [Frozen] ICapabilitiesService capabilitiesService,
            [Frozen] IFrameworkService frameworkService,
            [Frozen] IManageFiltersService manageFiltersService,
            [Frozen] ISolutionsFilterService solutionsFilterService,
            [Frozen] IUrlHelper mockUrlHelper,
            [Frozen] IPdfService mockPdfService,
            [Frozen] IIntegrationsService integrationsService)
        {
            organisationsService.GetOrganisationByInternalIdentifier(primaryOrganisationInternalId)
                .Returns(Task.FromResult(organisation));
            filterDetailsModel = null;
            manageFiltersService.GetFilterDetails(organisation.Id, filterId)
                .Returns(Task.FromResult(filterDetailsModel));
            var controller = CreateController(
                organisationsService,
                capabilitiesService,
                frameworkService,
                manageFiltersService,
                solutionsFilterService,
                mockPdfService,
                integrationsService,
                primaryOrganisationInternalId);
            controller.Url = mockUrlHelper;

            var result = await controller.DeleteFilter(filterId);
            var actualResult = result.Should().BeOfType<RedirectToActionResult>().Subject;

            actualResult.ControllerName.Should().Be(typeof(ManageFiltersController).ControllerName());
            actualResult.ActionName.Should()
                .Be(nameof(DashboardController.Index));
        }

        [Theory]
        [MockAutoData]
        public static async Task Post_DeleteFilter_ReturnsRedirectToActionResult(
            string primaryOrganisationInternalId,
            FilterDetailsModel filterDetailsModel,
            DeleteFilterModel deleteFilterModel,
            Organisation organisation,
            [Frozen] IOrganisationsService organisationsService,
            [Frozen] ICapabilitiesService capabilitiesService,
            [Frozen] IFrameworkService frameworkService,
            [Frozen] IManageFiltersService manageFiltersService,
            [Frozen] ISolutionsFilterService solutionsFilterService,
            [Frozen] IUrlHelper mockUrlHelper,
            [Frozen] IPdfService mockPdfService,
            [Frozen] IIntegrationsService integrationsService)
        {
            organisationsService.GetOrganisationByInternalIdentifier(primaryOrganisationInternalId)
                .Returns(Task.FromResult(organisation));
            deleteFilterModel.FilterId = filterDetailsModel.Id;
            manageFiltersService.GetFilterDetails(organisation.Id, deleteFilterModel.FilterId)
                .Returns(Task.FromResult(filterDetailsModel));
            var controller = CreateController(
                organisationsService,
                capabilitiesService,
                frameworkService,
                manageFiltersService,
                solutionsFilterService,
                mockPdfService,
                integrationsService,
                primaryOrganisationInternalId);
            controller.Url = mockUrlHelper;

            var result = await controller.DeleteFilter(deleteFilterModel);
            var actualResult = result.Should().BeOfType<RedirectToActionResult>().Subject;

            actualResult.ActionName.Should().Be(nameof(ManageFiltersController.Index));
            await manageFiltersService.Received(1).DeleteFilter(deleteFilterModel.FilterId);
        }

        [Theory]
        [MockAutoData]
        public static async void Get_MaximumShortlists_ReturnsExpectedResult(
            int filterId,
            FilterDetailsModel filterDetailsModel,
            [Frozen] IOrganisationsService organisationsService,
            [Frozen] ICapabilitiesService capabilitiesService,
            [Frozen] IFrameworkService frameworkService,
            [Frozen] IManageFiltersService manageFiltersService,
            [Frozen] ISolutionsFilterService solutionsFilterService,
            [Frozen] IUrlHelper mockUrlHelper,
            [Frozen] IPdfService mockPdfService,
            [Frozen] IIntegrationsService integrationsService,
            string primaryOrganisationInternalId,
            Organisation organisation)
        {
            organisationsService
                .GetOrganisationByInternalIdentifier(primaryOrganisationInternalId)
                .Returns(Task.FromResult(organisation));
            manageFiltersService
                .GetFilterDetails(organisation.Id, filterId)
                .Returns(Task.FromResult(filterDetailsModel));
            var controller = CreateController(
                organisationsService,
                capabilitiesService,
                frameworkService,
                manageFiltersService,
                solutionsFilterService,
                mockPdfService,
                integrationsService,
                primaryOrganisationInternalId);
            controller.Url = mockUrlHelper;

            var result = await controller.MaximumShortlists();

            var actualResult = result.Should().BeOfType<ViewResult>().Subject;
            actualResult.Model.Should().BeOfType<MaximumShortlistsModel>();
            ((MaximumShortlistsModel)actualResult.Model).OrganisationName.Should().Be(organisation.Name);
        }

        private static ManageFiltersController CreateController(
            IOrganisationsService organisationsService,
            ICapabilitiesService capabilitiesService,
            IFrameworkService frameworkService,
            IManageFiltersService manageFiltersService,
            ISolutionsFilterService solutionsFilterService,
            IPdfService pdfService,
            IIntegrationsService integrationsService,
            string primaryOrganisationInternalId)
        {
            return new ManageFiltersController(
                organisationsService,
                capabilitiesService,
                frameworkService,
                manageFiltersService,
                solutionsFilterService,
                pdfService,
                integrationsService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(
                            new ClaimsIdentity(
                                new Claim[]
                                {
                                    new(
                                        Framework.Constants.CatalogueClaims.PrimaryOrganisationInternalIdentifier,
                                        primaryOrganisationInternalId),
                                })),
                    },
                },
            };
        }
    }
}
