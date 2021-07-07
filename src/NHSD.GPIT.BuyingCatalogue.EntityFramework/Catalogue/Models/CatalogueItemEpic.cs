﻿using System;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;

namespace NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models
{
    public sealed class CatalogueItemEpic
    {
        public CatalogueItemId CatalogueItemId { get; set; }

        public Guid CapabilityId { get; set; }

        public string EpicId { get; set; }

        public int StatusId { get; set; }

        public DateTime LastUpdated { get; set; }

        public Guid LastUpdatedBy { get; set; }

        public Epic Epic { get; set; }

        public CatalogueItemEpicStatus Status { get; set; }
    }
}
