﻿using System;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Marketing.Models.Solution;
using NUnit.Framework;

// MJRTODO - Getting a namespace vs type clash when using Solution
namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Marketing.Models.SolutionX
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    internal static class SolutionStatusModelTests
    {
        [Test]
        public static void Constructor_NullCatalogueItem_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _ = new SolutionStatusModel(null));
        }
    }
}