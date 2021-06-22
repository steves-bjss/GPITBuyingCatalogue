﻿namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Order.Models.Order
{
    public class DeleteConfirmationModel : OrderingBaseModel
    {
        public DeleteConfirmationModel(string odsCode, EntityFramework.Models.Ordering.Order order)
        {
            BackLinkText = "Go back to all orders";
            BackLink = $"/order/organisation/{odsCode}";
            Title = $"Order {order.CallOffId} deleted";
            OdsCode = odsCode;
            Description = order.Description;
        }

        public string Description { get; set; }
    }
}