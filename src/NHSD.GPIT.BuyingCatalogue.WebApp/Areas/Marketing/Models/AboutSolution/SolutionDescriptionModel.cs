﻿using System;
using System.ComponentModel.DataAnnotations;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.BuyingCatalogue;
using NHSD.GPIT.BuyingCatalogue.WebApp.DataAttributes;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Marketing.Models.AboutSolution
{
    public class SolutionDescriptionModel : MarketingBaseModel
    {
        public SolutionDescriptionModel() : base(null)
        {
        }

        public SolutionDescriptionModel(CatalogueItem catalogueItem) : base(catalogueItem)
        {
            if (catalogueItem is null)
                throw new ArgumentNullException(nameof(catalogueItem));

            BackLink = $"/marketing/supplier/solution/{CatalogueItem.CatalogueItemId}";                                    
            Summary = catalogueItem.Solution.Summary;
            Description = catalogueItem.Solution.FullDescription;
            Link = catalogueItem.Solution.AboutUrl;            
        }

        public override bool? IsComplete => !string.IsNullOrWhiteSpace(CatalogueItem?.Solution?.Summary);

        [Required(ErrorMessage = "Enter a Summary")]
        [StringLength(350)]
        public string Summary { get; set; }

        [StringLength(1100)]
        [LabelText("Full Description (optional)")]
        [LabelHint("Provide a more detailed description of your Catalogue Solution, for example how it can be used and how it benefits users.")]
        [TextAreaRows(17)]
        public string Description { get; set; }

        [StringLength(1000)]
        [Url]
        public string Link { get; set; }      
    }
}
