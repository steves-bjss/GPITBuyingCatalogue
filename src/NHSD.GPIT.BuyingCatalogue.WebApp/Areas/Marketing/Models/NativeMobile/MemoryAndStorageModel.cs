﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.BuyingCatalogue;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Marketing.Models.NativeMobile
{
    public class MemoryAndStorageModel : MarketingBaseModel
    {
        public MemoryAndStorageModel() : base(null)
        {
        }

        public MemoryAndStorageModel(CatalogueItem catalogueItem) : base(catalogueItem)
        {
            if (catalogueItem is null)
                throw new ArgumentNullException(nameof(catalogueItem));

            BackLink = $"/marketing/supplier/solution/{CatalogueItem.CatalogueItemId}/section/native-mobile";

            MemorySizes = new List<SelectListItem>
            {
                new SelectListItem{ Text = "Please select"},
                new SelectListItem{ Text = "256MB", Value = "256MB"},
                new SelectListItem{ Text = "512MB", Value = "512MB"},
                new SelectListItem{ Text = "1GB", Value = "1GB"},
                new SelectListItem{ Text = "2GB", Value = "2GB"},
                new SelectListItem{ Text = "4GB", Value = "4GB"},
                new SelectListItem{ Text = "8GB", Value = "8GB"},
                new SelectListItem{ Text = "16GB or higher", Value = "16GB or higher"}
            };

            SelectedMemorySize = ClientApplication?.MobileMemoryAndStorage?.MinimumMemoryRequirement;
            Description = ClientApplication?.MobileMemoryAndStorage?.Description;
        }

        public override bool? IsComplete
        {
            get 
            {
                return !string.IsNullOrWhiteSpace(ClientApplication?.MobileMemoryAndStorage?.MinimumMemoryRequirement) &&
                  !string.IsNullOrWhiteSpace(ClientApplication?.MobileMemoryAndStorage?.Description);
            }
        }

        public string SelectedMemorySize { get; set; }
        public List<SelectListItem> MemorySizes { get; set; }

        public string Description { get; set; }
    }
}