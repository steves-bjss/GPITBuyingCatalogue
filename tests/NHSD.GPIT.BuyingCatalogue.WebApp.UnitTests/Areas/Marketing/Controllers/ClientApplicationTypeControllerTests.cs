﻿using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.GPIT.BuyingCatalogue.Framework.Logging;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Marketing.Controllers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Marketing.Models.ClientApplicationType;
using NUnit.Framework;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Marketing.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    internal static class ClientApplicationTypeControllerTests
    {
        private static string[] InvalidStrings = { null, string.Empty, "    " };

        [Test]
        public static void ClassIsCorrectlyDecorated()
        {
            typeof(ClientApplicationTypeController).Should().BeDecoratedWith<AreaAttribute>(x => x.RouteValue == "Marketing");
        }

        [Test]
        public static void Constructor_NullLogging_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _ = new ClientApplicationTypeController(null, Mock.Of<ISolutionsService>()));
        }

        [Test]
        public static void Constructor_NullSolutionService_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _ = new ClientApplicationTypeController(Mock.Of<ILogWrapper<ClientApplicationTypeController>>(), null));
        }

        [Test]
        [TestCaseSource(nameof(InvalidStrings))]
        public static void Get_ClientApplicationTypes_InvalidId_ThrowsException(string id)
        {
            var controller = new ClientApplicationTypeController(Mock.Of<ILogWrapper<ClientApplicationTypeController>>(), Mock.Of<ISolutionsService>());

            Assert.ThrowsAsync<ArgumentException>(() => controller.ClientApplicationTypes(id));
        }

        [Test]
        public static void Post_ClientApplicationTypes_NullModel_ThrowsException()
        {
            var controller = new ClientApplicationTypeController(Mock.Of<ILogWrapper<ClientApplicationTypeController>>(), Mock.Of<ISolutionsService>());

            Assert.ThrowsAsync<ArgumentNullException>(() => controller.ClientApplicationTypes((ClientApplicationTypesModel)null));
        }

        [Test]
        [TestCaseSource(nameof(InvalidStrings))]
        public static void Get_BrowserBased_InvalidId_ThrowsException(string id)
        {
            var controller = new ClientApplicationTypeController(Mock.Of<ILogWrapper<ClientApplicationTypeController>>(), Mock.Of<ISolutionsService>());

            Assert.ThrowsAsync<ArgumentException>(() => controller.BrowserBased(id));
        }

        [Test]
        [TestCaseSource(nameof(InvalidStrings))]
        public static void Get_NativeMobile_InvalidId_ThrowsException(string id)
        {
            var controller = new ClientApplicationTypeController(Mock.Of<ILogWrapper<ClientApplicationTypeController>>(), Mock.Of<ISolutionsService>());

            Assert.ThrowsAsync<ArgumentException>(() => controller.NativeMobile(id));
        }

        [Test]
        [TestCaseSource(nameof(InvalidStrings))]
        public static void Get_NativeDesktop_InvalidId_ThrowsException(string id)
        {
            var controller = new ClientApplicationTypeController(Mock.Of<ILogWrapper<ClientApplicationTypeController>>(), Mock.Of<ISolutionsService>());

            Assert.ThrowsAsync<ArgumentException>(() => controller.NativeDesktop(id));
        }
    }
}