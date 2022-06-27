﻿using System;
using System.Collections.Generic;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.Admin.ListPrices.Base.Tiered;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Controllers;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.Admin.ListPrices.AssociatedService.Tiered
{
    public sealed class ListPriceType : ListPriceTypeBase
    {
        private static readonly CatalogueItemId SolutionId = new(99998, "001");
        private static readonly CatalogueItemId AssociatedServiceId = new(99998, "S-997");

        private static readonly Dictionary<string, string> Parameters = new()
        {
            { nameof(SolutionId), SolutionId.ToString() },
            { nameof(AssociatedServiceId), AssociatedServiceId.ToString() },
        };

        public ListPriceType(LocalWebApplicationFactory factory)
            : base(
                factory,
                typeof(AssociatedServiceListPriceController),
                Parameters)
        {
        }

        protected override CatalogueItemId CatalogueItemId => AssociatedServiceId;

        protected override Type Controller => typeof(AssociatedServiceListPriceController);
    }
}
