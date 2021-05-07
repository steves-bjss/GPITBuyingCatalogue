﻿using System;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.BuyingCatalogue;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Marketing.Models.NativeMobile;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Marketing.Models.ClientApplicationType
{
    public class NativeMobileModel : MarketingBaseModel
    { 
        public NativeMobileModel(CatalogueItem catalogueItem) : base(catalogueItem)
        {
            if (catalogueItem is null)
                throw new ArgumentNullException(nameof(catalogueItem));

            BackLink = $"/marketing/supplier/solution/{CatalogueItem.CatalogueItemId}";                 
        }

        public override bool? IsComplete
        {
            get
            {
                return new OperatingSystemsModel(CatalogueItem).IsComplete.GetValueOrDefault() &&
                    new MobileFirstApproachModel(CatalogueItem).IsComplete.GetValueOrDefault() &&
                    new MemoryAndStorageModel(CatalogueItem).IsComplete.GetValueOrDefault();
            }
        }                

        public string SupportedOperatingSystemsStatus
        {
            get { return GetStatus(new OperatingSystemsModel(CatalogueItem)); }
        }

        public string MobileFirstApproachStatus
        {
            get { return GetStatus(new MobileFirstApproachModel(CatalogueItem)); }
        }

        public string ConnectivityStatus
        {
            get { return GetStatus(new ConnectivityModel(CatalogueItem)); }
        }

        public string MemoryAndStorageStatus
        {
            get { return GetStatus(new MemoryAndStorageModel(CatalogueItem)); }
        }

        public string ThirdPartyStatus
        {
            get { return GetStatus(new ThirdPartyModel(CatalogueItem)); }
        }

        public string HardwareRequirementsStatus
        {
            get { return GetStatus(new HardwareRequirementsModel(CatalogueItem)); }
        }

        public string AdditionalInformationStatus
        {
            get { return GetStatus(new AdditionalInformationModel(CatalogueItem)); }
        }
    }
}