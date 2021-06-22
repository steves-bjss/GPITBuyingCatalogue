﻿using System.Collections.Generic;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.Ordering;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Order.Models.CatalogueSolutions
{
    public class CatalogueSolutionsModel : OrderingBaseModel
    {
        public CatalogueSolutionsModel(string odsCode, EntityFramework.Models.Ordering.Order order, List<OrderItem> orderItems)
        {
            BackLink = $"/order/organisation/{odsCode}/order/{order.CallOffId}";
            BackLinkText = "Go back";
            Title = $"Catalogue Solution for {order.CallOffId}";
            OdsCode = odsCode;
            OrderDescription = order.Description;
            CallOffId = order.CallOffId.ToString();
            OrderItems = orderItems;
        }

        public string OrderDescription { get; set; }

        public string CallOffId { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}