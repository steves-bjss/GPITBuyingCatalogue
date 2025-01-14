﻿using System;
using System.Collections.Generic;
using System.Linq;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Extensions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.Framework.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Models.AdminManageOrders;
using NHSD.GPIT.BuyingCatalogue.WebApp.Models;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.ManageOrders
{
    public class ManageOrdersDashboardModel : NavBaseModel
    {
        public ManageOrdersDashboardModel()
        {
        }

        public ManageOrdersDashboardModel(IList<AdminManageOrder> orders, IEnumerable<EntityFramework.Catalogue.Models.Framework> frameworks, PageOptions options, OrderStatus? status, string framework)
        {
            Orders = orders.ToList();
            Options = options;
            SetAvailableFrameworks(frameworks);
            SelectedStatus = status;
            SelectedFramework = framework;
            SetFilterCount();
        }

        public IList<AdminManageOrder> Orders { get; set; }

        public PageOptions Options { get; set; }

        public int FilterCount { get; set; }

        public string SelectedFramework { get; set; }

        public IEnumerable<SelectOption<string>> AvailableFrameworks { get; set; }

        public OrderStatus? SelectedStatus { get; set; }

        public IEnumerable<SelectOption<OrderStatus>> AvailableStatus =>
            Enum.GetValues<OrderStatus>()
                .Select(
                    x => new SelectOption<OrderStatus>
                    {
                        Value = x,
                        Text = x.EnumMemberName(),
                    }).ToList();

        private void SetAvailableFrameworks(IEnumerable<EntityFramework.Catalogue.Models.Framework> frameworks)
        {
            AvailableFrameworks = frameworks
                .OrderBy(f => f.ShortName)
                .Select(
                    f => new SelectOption<string>
                    {
                        Value = f.Id,
                        Text = $"{f.ShortName}{(f.IsExpired ? " (expired)" : string.Empty)}",
                    })
                .ToList();
        }

        private void SetFilterCount()
        {
            FilterCount = 0;
            if (!string.IsNullOrEmpty(SelectedFramework))
                FilterCount++;
            if (SelectedStatus is not null)
                FilterCount++;
        }
    }
}
