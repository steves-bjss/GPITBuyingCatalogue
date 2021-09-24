﻿namespace NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models
{
    public sealed class PricingUnit
    {
        public short Id { get; set; }

        public string TierName { get; set; }

        public string Description { get; set; }

        public string Definition { get; set; }
    }
}
