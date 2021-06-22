﻿using System.ComponentModel.DataAnnotations;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Order.Models.CatalogueSolutions
{
    public class SelectFlatDeclarativeQuantityModel : OrderingBaseModel
    {
        public SelectFlatDeclarativeQuantityModel()
        {
        }

        public SelectFlatDeclarativeQuantityModel(string odsCode, string callOffId, string solutionName, int? quantity)
        {
            BackLink = $"/order/organisation/{odsCode}/order/{callOffId}/catalogue-solutions/select/solution/price/recipients/date";
            BackLinkText = "Go back";
            Title = $"Quantity of {solutionName} for {callOffId}";
            Quantity = quantity.ToString();
        }

        [Required(ErrorMessage = "Enter a quantity")]
        public string Quantity { get; set; }

        public (int? Quantity, string Error) GetQuantity()
        {
            int quantity;

            if (!int.TryParse(Quantity, out quantity))
                return (null, "Quantity must be a number");

            if (quantity < 1)
                return (null, "Quantity must be greater than zero");

            return (quantity, null);
        }
    }
}