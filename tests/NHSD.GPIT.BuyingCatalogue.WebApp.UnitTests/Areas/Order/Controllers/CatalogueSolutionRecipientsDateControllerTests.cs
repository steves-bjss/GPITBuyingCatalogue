﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Orders;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Session;
using NHSD.GPIT.BuyingCatalogue.Test.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Order.Controllers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Order.Models.CatalogueSolutionRecipientsDate;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Order.Controllers
{
    public static class CatalogueSolutionRecipientsDateControllerTests
    {
        [Fact]
        public static void ClassIsCorrectlyDecorated()
        {
            typeof(CatalogueSolutionRecipientsDateController).Should().BeDecoratedWith<AuthorizeAttribute>();
            typeof(CatalogueSolutionRecipientsDateController).Should().BeDecoratedWith<AreaAttribute>(a => a.RouteValue == "Order");
        }

        [Fact]
        public static void Constructors_VerifyGuardClauses()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var assertion = new GuardClauseAssertion(fixture);
            var constructors = typeof(CatalogueSolutionRecipientsDateController).GetConstructors();

            assertion.Verify(constructors);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_SelectSolutionServiceRecipientsDate_ReturnsExpectedResult(
            string odsCode,
            DateTime? defaultDeliveryDate,
            CreateOrderItemModel state,
            [Frozen] Mock<IOrderSessionService> orderSessionServiceMock,
            CatalogueSolutionRecipientsDateController controller)
        {
            var expectedViewData = new SelectSolutionServiceRecipientsDateModel(odsCode, state, defaultDeliveryDate);
            orderSessionServiceMock.Setup(s => s.GetOrderStateFromSession(state.CallOffId)).Returns(state);

            var actualResult = await controller.SelectSolutionServiceRecipientsDate(odsCode, state.CallOffId);

            actualResult.Should().BeOfType<ViewResult>();
            actualResult.As<ViewResult>().ViewData.Model.Should().BeEquivalentTo(expectedViewData);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_SelectSolutionServiceRecipientsDate_InvalidDate_ReturnsErrorResult(
            string odsCode,
            CallOffId callOffId,
            CatalogueSolutionRecipientsDateController controller)
        {
            var model = new SelectSolutionServiceRecipientsDateModel { Day = "ABC", };

            var actualResult = await controller.SelectSolutionServiceRecipientsDate(odsCode, callOffId, model);

            actualResult.Should().BeOfType<ViewResult>();
            actualResult.As<ViewResult>().ViewData.ModelState.ValidationState.Should().Be(ModelValidationState.Invalid);

            actualResult.As<ViewResult>()
                .ViewData.ModelState.Keys.Single()
                .Should()
                .Be("Day");

            actualResult.As<ViewResult>()
                .ViewData.ModelState.Values.Single()
                .Errors.Single()
                .ErrorMessage.Should()
                .Be("Planned delivery date must be a real date");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_SelectSolutionServiceRecipientsDate_Declarative_CorrectlyRedirects(
            string odsCode,
            CreateOrderItemModel state,
            [Frozen] Mock<IOrderSessionService> orderSessionServiceMock,
            CatalogueSolutionRecipientsDateController controller)
        {
            var model = new SelectSolutionServiceRecipientsDateModel
            {
                Day = DateTime.UtcNow.AddDays(1).Day.ToString(),
                Month = DateTime.UtcNow.AddDays(1).Month.ToString(),
                Year = DateTime.UtcNow.AddDays(1).Year.ToString(),
            };

            state.CataloguePrice.ProvisioningType = ProvisioningType.Declarative;

            orderSessionServiceMock.Setup(s => s.GetOrderStateFromSession(state.CallOffId)).Returns(state);

            var actualResult = await controller.SelectSolutionServiceRecipientsDate(odsCode, state.CallOffId, model);

            actualResult.Should().BeOfType<RedirectToActionResult>();
            actualResult.As<RedirectToActionResult>().ActionName.Should().Be(nameof(CatalogueSolutionsController.SelectFlatDeclarativeQuantity));
            actualResult.As<RedirectToActionResult>().ControllerName.Should().Be(typeof(CatalogueSolutionsController).ControllerName());
            actualResult.As<RedirectToActionResult>().RouteValues.Should().BeEquivalentTo(new RouteValueDictionary { { "odsCode", odsCode }, { "callOffId", state.CallOffId } });
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_SelectSolutionServiceRecipientsDate_OnDemand_CorrectlyRedirects(
            string odsCode,
            CreateOrderItemModel state,
            [Frozen] Mock<IOrderSessionService> orderSessionServiceMock,
            CatalogueSolutionRecipientsDateController controller)
        {
            var model = new SelectSolutionServiceRecipientsDateModel
            {
                Day = DateTime.UtcNow.AddDays(1).Day.ToString(),
                Month = DateTime.UtcNow.AddDays(1).Month.ToString(),
                Year = DateTime.UtcNow.AddDays(1).Year.ToString(),
            };

            state.CataloguePrice.ProvisioningType = ProvisioningType.OnDemand;

            orderSessionServiceMock.Setup(s => s.GetOrderStateFromSession(state.CallOffId)).Returns(state);

            var actualResult = await controller.SelectSolutionServiceRecipientsDate(odsCode, state.CallOffId, model);

            actualResult.Should().BeOfType<RedirectToActionResult>();
            actualResult.As<RedirectToActionResult>().ActionName.Should().Be(nameof(CatalogueSolutionsController.SelectFlatOnDemandQuantity));
            actualResult.As<RedirectToActionResult>().ControllerName.Should().Be(typeof(CatalogueSolutionsController).ControllerName());
            actualResult.As<RedirectToActionResult>().RouteValues.Should().BeEquivalentTo(new RouteValueDictionary { { "odsCode", odsCode }, { "callOffId", state.CallOffId } });
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_SelectSolutionServiceRecipientsDate_Patient_CorrectlyRedirects(
            string odsCode,
            CreateOrderItemModel state,
            [Frozen] Mock<IOrderSessionService> orderSessionServiceMock,
            CatalogueSolutionRecipientsDateController controller)
        {
            var model = new SelectSolutionServiceRecipientsDateModel
            {
                Day = DateTime.UtcNow.AddDays(1).Day.ToString(),
                Month = DateTime.UtcNow.AddDays(1).Month.ToString(),
                Year = DateTime.UtcNow.AddDays(1).Year.ToString(),
            };

            state.CataloguePrice.ProvisioningType = ProvisioningType.Patient;

            orderSessionServiceMock.Setup(s => s.GetOrderStateFromSession(state.CallOffId)).Returns(state);

            var actualResult = await controller.SelectSolutionServiceRecipientsDate(odsCode, state.CallOffId, model);

            actualResult.Should().BeOfType<RedirectToActionResult>();
            actualResult.As<RedirectToActionResult>().ActionName.Should().Be(nameof(CatalogueSolutionsController.EditSolution));
            actualResult.As<RedirectToActionResult>().ControllerName.Should().Be(typeof(CatalogueSolutionsController).ControllerName());
            actualResult.As<RedirectToActionResult>().RouteValues.Should().BeEquivalentTo(new RouteValueDictionary { { "odsCode", odsCode }, { "callOffId", state.CallOffId }, { "CatalogueItemId", state.CatalogueItemId } });
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_SelectSolutionServiceRecipientsDate_SetsDatesAndSession(
            string odsCode,
            CreateOrderItemModel state,
            [Frozen] Mock<IOrderSessionService> orderSessionServiceMock,
            [Frozen] Mock<IDefaultDeliveryDateService> defaultDeliveryDateServiceMock,
            CatalogueSolutionRecipientsDateController controller)
        {
            var model = new SelectSolutionServiceRecipientsDateModel
            {
                Day = DateTime.UtcNow.AddDays(1).Day.ToString(),
                Month = DateTime.UtcNow.AddDays(1).Month.ToString(),
                Year = DateTime.UtcNow.AddDays(1).Year.ToString(),
            };

            orderSessionServiceMock.Setup(s => s.GetOrderStateFromSession(state.CallOffId)).Returns(state);

            CreateOrderItemModel updatedState = null;

            orderSessionServiceMock.Setup(s => s.SetOrderStateToSession(It.IsAny<CreateOrderItemModel>()))
                .Callback<CreateOrderItemModel>(s => updatedState = s);

            await controller.SelectSolutionServiceRecipientsDate(odsCode, state.CallOffId, model);

            updatedState.PlannedDeliveryDate.Should().Be(DateTime.UtcNow.AddDays(1).Date);

            defaultDeliveryDateServiceMock.Verify(c => c.SetDefaultDeliveryDate(state.CallOffId, state.CatalogueItemId.GetValueOrDefault(), DateTime.UtcNow.AddDays(1).Date), Times.Once());
        }
    }
}
