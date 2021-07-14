﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;

namespace NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models
{
    public partial class CatalogueItem
    {
        public virtual CatalogueItemCapability CatalogueItemCapability(
            Guid capabilityId) =>
            Solution?.SolutionCapabilities?.FirstOrDefault(
                sc => sc.Capability != null && sc.Capability.Id == capabilityId)
            ?? new CatalogueItemCapability { CatalogueItemId = CatalogueItemId };

        public virtual string[] Features() =>
            HasFeatures() ? JsonConvert.DeserializeObject<string[]>(Solution.Features) : null;

        public MarketingContact FirstContact() =>
            Solution?.MarketingContacts?.FirstOrDefault() ?? new MarketingContact();

        public virtual IList<string> Frameworks() =>
            Solution?.FrameworkSolutions?.Any() == true
                ? Solution?.FrameworkSolutions.Select(s => s.Framework?.ShortName).ToList()
                : new List<string>();

        public virtual bool HasAdditionalServices() => AdditionalService != null
            || Supplier?.CatalogueItems?.Any(c => c.AdditionalService is not null) == true;

        public virtual bool HasAssociatedServices() => AssociatedService != null
            || Supplier?.CatalogueItems?.Any(c => c.AssociatedService is not null) == true;

        public virtual bool HasCapabilities() => Solution?.SolutionCapabilities?.Any() == true;

        public virtual bool HasClientApplication() => !string.IsNullOrWhiteSpace(Solution?.ClientApplication);

        public virtual bool HasDevelopmentPlans() => !string.IsNullOrWhiteSpace(Solution?.RoadMap);

        public virtual bool HasFeatures() => !string.IsNullOrWhiteSpace(Solution?.Features);

        public virtual bool HasHosting() => !string.IsNullOrWhiteSpace(Solution?.Hosting);

        public virtual bool HasImplementationDetail() => !string.IsNullOrWhiteSpace(Solution?.ImplementationDetail);

        public virtual bool HasInteroperability() => !string.IsNullOrWhiteSpace(Solution?.Integrations);

        public virtual bool HasListPrice() => CataloguePrices?.Any() == true;

        public virtual bool HasServiceLevelAgreement() => !string.IsNullOrWhiteSpace(Solution?.ServiceLevelAgreement);

        public virtual bool HasSupplierDetails() => Supplier != null;

        public virtual bool? IsFoundation() => Solution?.FrameworkSolutions?.Any(f => f.IsFoundation);

        public MarketingContact SecondContact() =>
            Solution?.MarketingContacts?.Skip(1).FirstOrDefault() ?? new MarketingContact();

        public string CatalogueItemName(CatalogueItemId catalogueItemId) => Supplier?.CatalogueItems
            .FirstOrDefault(c => c.CatalogueItemId == catalogueItemId)
            ?.Name;

        public string AdditionalServiceDescription(CatalogueItemId catalogueItemId) => Supplier?.CatalogueItems
            .FirstOrDefault(c => c.CatalogueItemId == catalogueItemId)
            ?.AdditionalService?.FullDescription;
    }
}
