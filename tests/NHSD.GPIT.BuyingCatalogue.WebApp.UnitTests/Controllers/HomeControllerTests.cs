﻿using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.GPIT.BuyingCatalogue.Framework.Logging;
using NHSD.GPIT.BuyingCatalogue.WebApp.Controllers;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Controllers
{
    public static class HomeControllerTests
    {
        [Fact]
        public static void Constructor_NullLogging_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _ = new HomeController(null));
        }

        [Fact]
        public static void Get_Index_ReturnsDefaultView()
        {
            var controller = new HomeController(Mock.Of<ILogWrapper<HomeController>>());

            var result = controller.Index();

            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.Null(((ViewResult)result).ViewName);
        }

        [Fact]
        public static void Get_Error500_ReturnsDefaultErrorView()
        {
            var controller = new HomeController(Mock.Of<ILogWrapper<HomeController>>());

            var result = controller.Error(500);

            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.Null(((ViewResult)result).ViewName);
        }

        [Fact]
        public static void Get_ErrorNullStatus_ReturnsDefaultErrorView()
        {
            var controller = new HomeController(Mock.Of<ILogWrapper<HomeController>>());

            var result = controller.Error(null);

            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.Null(((ViewResult)result).ViewName);
        }

        [Fact]
        public static void Get_Error404_ReturnsPageNotFound()
        {
            var controller = new HomeController(Mock.Of<ILogWrapper<HomeController>>());

            IFeatureCollection features = new FeatureCollection();
            features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature { OriginalPath = "BAD" });

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext(features)
            };

            var result = controller.Error(404);

            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.Equal("PageNotFound", ((ViewResult)result).ViewName);
            Assert.Equal("Incorrect url BAD", ((ViewResult)result).ViewData["BadUrl"]);
        }
    }
}
