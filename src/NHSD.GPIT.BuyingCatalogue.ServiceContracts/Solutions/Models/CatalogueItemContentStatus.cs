﻿namespace NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions.Models
{
    public sealed class CatalogueItemContentStatus
    {
        public static bool ShowDescription => true;

        public static bool ShowStandards => true;

        public static bool ShowListPrice => true;

        public static bool ShowDevelopmentPlans => true;

        public static bool ShowCapabilities => true;

        public static bool ShowClientApplications => true;

        public static bool ShowHosting => true;

        public static bool ShowServiceLevelAgreements => true;

        public static bool ShowSupplierDetails => true;

        public bool ShowFeatures { get; init; }

        public bool ShowAdditionalServices { get; init; }

        public bool ShowAssociatedServices { get; init; }

        public bool ShowInteroperability { get; init; }

        public bool ShowImplementation { get; init; }
    }
}