﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Orders;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Order.Models.FundingSource;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Order.Controllers
{
    [Authorize]
    [Area("Order")]
    [Route("order/organisation/{odsCode}/order/{callOffId}/funding-source")]
    public sealed class FundingSourceController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IFundingSourceService fundingSourceService;

        public FundingSourceController(
            IOrderService orderService,
            IFundingSourceService fundingSourceService)
        {
            this.orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            this.fundingSourceService = fundingSourceService ?? throw new ArgumentNullException(nameof(fundingSourceService));
        }

        [HttpGet]
        public async Task<IActionResult> FundingSource(string odsCode, CallOffId callOffId)
        {
            var order = await orderService.GetOrder(callOffId);

            return View(new FundingSourceModel(odsCode, callOffId, order.FundingSourceOnlyGms));
        }

        [HttpPost]
        public async Task<IActionResult> FundingSource(string odsCode, CallOffId callOffId, FundingSourceModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var onlyGms = model.FundingSourceOnlyGms.EqualsIgnoreCase("Yes");

            await fundingSourceService.SetFundingSource(callOffId, onlyGms);

            return RedirectToAction(
                nameof(Order),
                typeof(OrderController).ControllerName(),
                new { odsCode, callOffId });
        }
    }
}