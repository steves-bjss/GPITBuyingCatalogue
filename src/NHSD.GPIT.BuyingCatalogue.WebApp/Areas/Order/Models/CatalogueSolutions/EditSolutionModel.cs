﻿using System.Linq;
using NHSD.GPIT.BuyingCatalogue.Framework.Constants;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Models;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Order.Models.CatalogueSolutions
{
    public class EditSolutionModel : OrderingBaseModel
    {
        public EditSolutionModel()
        {
        }

        public EditSolutionModel(string odsCode, string callOffId, string id, CreateOrderItemModel createOrderItemModel)
        {
            BackLink = $"/order/organisation/{odsCode}/order/{callOffId}/catalogue-solutions";
            BackLinkText = "Go back";
            Title = $"{createOrderItemModel.CatalogueItemName} information for {callOffId}";
            OdsCode = odsCode;
            CallOffId = callOffId;
            OrderItem = createOrderItemModel;
            OrderItem.ServiceRecipients = OrderItem.ServiceRecipients.Where(x => x.Selected).ToList();
            CurrencySymbol = CurrencyCodeSigns.Code[createOrderItemModel.CurrencyCode];
        }

        public string CallOffId { get; set; }

        public CreateOrderItemModel OrderItem { get; set; }

        public string CurrencySymbol { get; set; }
    }
}