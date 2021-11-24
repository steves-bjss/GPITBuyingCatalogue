﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.AssociatedServices;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.ListPrices;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Models.AssociatedServices;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Suppliers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.AssociatedServices;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.ListPriceModels;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Area("Admin")]
    [Route("admin/catalogue-solutions/manage/{solutionId}/associated-services")]
    public sealed class AssociatedServicesController : Controller
    {
        private readonly ISolutionsService solutionsService;
        private readonly ISuppliersService suppliersService;
        private readonly IAssociatedServicesService associatedServicesService;
        private readonly IListPricesService listPricesService;

        public AssociatedServicesController(
            ISolutionsService solutionsService,
            ISuppliersService suppliersService,
            IAssociatedServicesService associatedServicesService,
            IListPricesService listPricesService)
        {
            this.solutionsService = solutionsService ?? throw new ArgumentNullException(nameof(solutionsService));
            this.suppliersService = suppliersService ?? throw new ArgumentNullException(nameof(suppliersService));
            this.associatedServicesService = associatedServicesService ?? throw new ArgumentNullException(nameof(associatedServicesService));
            this.listPricesService = listPricesService ?? throw new ArgumentNullException(nameof(listPricesService));
        }

        [HttpGet]
        public async Task<IActionResult> AssociatedServices(CatalogueItemId solutionId)
        {
            var solution = await solutionsService.GetSolution(solutionId);
            if (solution is null)
                return BadRequest($"No Solution found for Id: {solutionId}");

            var associatedServices = await associatedServicesService.GetAssociatedServicesForSupplier(solution.Supplier.Id);

            var model = new AssociatedServicesModel(solution, associatedServices)
            {
                BackLink = Url.Action(
                    nameof(CatalogueSolutionsController.ManageCatalogueSolution),
                    typeof(CatalogueSolutionsController).ControllerName(),
                    new { solutionId }),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssociatedServices(CatalogueItemId solutionId, AssociatedServicesModel model)
        {
            if (model.SelectableAssociatedServices is not null)
            {
                var associatedServices = model.SelectableAssociatedServices.Where(a => a.Selected).Select(a => a.CatalogueItemId);
                await associatedServicesService.RelateAssociatedServicesToSolution(solutionId, associatedServices);
            }

            return RedirectToAction(
                nameof(CatalogueSolutionsController.ManageCatalogueSolution),
                typeof(CatalogueSolutionsController).ControllerName(),
                new { solutionId });
        }

        [HttpGet("add-associated-service")]
        public async Task<IActionResult> AddAssociatedService(CatalogueItemId solutionId)
        {
            var solution = await solutionsService.GetSolution(solutionId);
            if (solution is null)
                return BadRequest($"No Solution found for Id: {solutionId}");

            var model = new AddAssociatedServiceModel(solution)
            {
                BackLink = Url.Action(nameof(AssociatedServices), new { solutionId }),
            };

            return View(model);
        }

        [HttpPost("add-associated-service")]
        public async Task<IActionResult> AddAssociatedService(CatalogueItemId solutionId, AddAssociatedServiceModel model)
        {
            if (await associatedServicesService.AssociatedServiceExistsWithNameForSupplier(model.Name, solutionId.SupplierId))
                ModelState.AddModelError(nameof(AddAssociatedServiceModel.Name), "Associated Service name already exists. Enter a different name");

            var solution = await solutionsService.GetSolution(solutionId);

            if (!ModelState.IsValid)
            {
                return View(new AddAssociatedServiceModel(solution));
            }

            var newModel = new AssociatedServicesDetailsModel
            {
                Name = model.Name,
                Description = model.Description,
                OrderGuidance = model.OrderGuidance,
                UserId = User.UserId(),
            };

            var associatedServiceId = await associatedServicesService.AddAssociatedService(solution, newModel);

            return RedirectToAction(
                nameof(EditAssociatedService),
                new
                {
                    solutionId = solution.Id,
                    associatedServiceId,
                });
        }

        [HttpGet("{associatedServiceId}/edit-associated-service")]
        public async Task<IActionResult> EditAssociatedService(CatalogueItemId solutionId, CatalogueItemId associatedServiceId)
        {
            var solution = await solutionsService.GetSolution(solutionId);
            if (solution is null)
                return BadRequest($"No Solution found for Id: {solutionId}");

            var associatedService = await associatedServicesService.GetAssociatedService(associatedServiceId);
            if (associatedService is null)
                return BadRequest($"No Associated Service found for Id: {associatedServiceId}");

            var model = new EditAssociatedServiceModel(solution, associatedService)
            {
                BackLink = Url.Action(nameof(AssociatedServices), new { solutionId }),
            };

            return View(model);
        }

        [HttpGet("{associatedServiceId}/edit-associated-service-details")]
        public async Task<IActionResult> EditAssociatedServiceDetails(CatalogueItemId solutionId, CatalogueItemId associatedServiceId)
        {
            var solution = await solutionsService.GetSolution(solutionId);
            if (solution is null)
                return BadRequest($"No Solution found for Id: {solutionId}");

            var associatedService = await associatedServicesService.GetAssociatedService(associatedServiceId);
            if (associatedService is null)
                return BadRequest($"No Associated Service found for Id: {associatedServiceId}");

            var model = new EditAssociatedServiceDetailsModel(solution, associatedService)
            {
                BackLink = Url.Action(nameof(EditAssociatedService), new { solutionId, associatedServiceId }),
            };

            return View(model);
        }

        [HttpPost("{associatedServiceId}/edit-associated-service-details")]
        public async Task<IActionResult> EditAssociatedServiceDetails(CatalogueItemId solutionId, CatalogueItemId associatedServiceId, EditAssociatedServiceDetailsModel model)
        {
            var solution = await solutionsService.GetSolution(solutionId);
            if (solution is null)
                return BadRequest($"No Solution found for Id: {solutionId}");

            var associatedService = await associatedServicesService.GetAssociatedService(associatedServiceId);
            if (associatedService is null)
                return BadRequest($"No Associated Service found for Id: {associatedServiceId}");

            if (await associatedServicesService.AssociatedServiceExistsWithNameForSupplier(model.Name, solutionId.SupplierId, associatedServiceId))
                ModelState.AddModelError(nameof(AddAssociatedServiceModel.Name), "Associated Service name already exists. Enter a different name");

            if (!ModelState.IsValid)
            {
                return View(new EditAssociatedServiceDetailsModel(solution, associatedService));
            }

            await associatedServicesService.EditDetails(
                associatedServiceId,
                new AssociatedServicesDetailsModel
                {
                    Name = model.Name,
                    Description = model.Description,
                    OrderGuidance = model.OrderGuidance,
                    UserId = User.UserId(),
                });

            return RedirectToAction(
                nameof(EditAssociatedService),
                new
                {
                    solutionId,
                    associatedServiceId,
                });
        }

        [HttpGet("{associatedServiceId}/list-prices")]
        public async Task<IActionResult> ManageListPrices(CatalogueItemId solutionId, CatalogueItemId associatedServiceId)
        {
            var solution = await solutionsService.GetSolution(solutionId);
            if (solution is null)
                return BadRequest($"No Solution found for Id: {solutionId}");

            var associatedService = await associatedServicesService.GetAssociatedService(associatedServiceId);
            if (associatedService is null)
                return BadRequest($"No Associated Service found for Id: {associatedServiceId}");

            var model = new ManageListPricesModel(associatedService, solutionId)
            {
                BackLink = Url.Action(
                    nameof(EditAssociatedService),
                    typeof(AssociatedServicesController).ControllerName(),
                    new { solutionId, associatedServiceId }),
                AddLink = Url.Action(
                    nameof(AddListPrice),
                    typeof(AssociatedServicesController).ControllerName(),
                    new { solutionId, associatedServiceId }),
            };

            return View(model);
        }

        [HttpGet("{associatedServiceId}/list-price/add-list-price")]
        public async Task<IActionResult> AddListPrice(CatalogueItemId solutionId, CatalogueItemId associatedServiceId)
        {
            var solution = await solutionsService.GetSolution(solutionId);
            if (solution is null)
                return BadRequest($"No Solution found for Id: {solutionId}");

            var associatedService = await associatedServicesService.GetAssociatedService(associatedServiceId);
            if (associatedService is null)
                return BadRequest($"No Associated Service found for Id: {associatedServiceId}");

            var model = new EditListPriceModel(associatedService)
            {
                Title = "Associated Service list price",
                BackLink = Url.Action(
                    nameof(ManageListPrices),
                    typeof(AssociatedServicesController).ControllerName(),
                    new { solutionId, associatedServiceId }),
            };

            return View("EditListPrice", model);
        }

        [HttpPost("{associatedServiceId}/list-price/add-list-price")]
        public async Task<IActionResult> AddListPrice(CatalogueItemId solutionId, CatalogueItemId associatedServiceId, EditListPriceModel model)
        {
            if (!ModelState.IsValid)
                return View("EditListPrice", model);

            var provisioningType = model.SelectedProvisioningType!.Value;
            var saveListPriceModel = new SaveListPriceModel
            {
                Price = model.Price,
                ProvisioningType = provisioningType,
                PricingUnit = model.GetPricingUnit(),
                TimeUnit = model.GetTimeUnit(provisioningType),
            };

            await listPricesService.SaveListPrice(associatedServiceId, saveListPriceModel);

            return RedirectToAction(nameof(ManageListPrices), new { solutionId, associatedServiceId });
        }

        [HttpGet("{associatedServiceId}/list-price/{listPriceId}")]
        public async Task<IActionResult> EditListPrice(CatalogueItemId solutionId, CatalogueItemId associatedServiceId, int listPriceId)
        {
            var associatedService = await listPricesService.GetCatalogueItemWithPrices(associatedServiceId);
            var cataloguePrice = associatedService.CataloguePrices.FirstOrDefault(cp => cp.CataloguePriceId == listPriceId);
            if (cataloguePrice is null)
                return RedirectToAction(nameof(ManageListPrices), new { solutionId, associatedServiceId });

            var editListPriceModel = new EditListPriceModel(associatedService, cataloguePrice, solutionId)
            {
                BackLink = Url.Action(nameof(ManageListPrices), new { solutionId, associatedServiceId }),
                DeleteLink = Url.Action(nameof(DeleteListPrice), new { solutionId, associatedServiceId, listPriceId }),
                Title = $"{associatedService.Name} list price",
            };

            return View("EditListPrice", editListPriceModel);
        }

        [HttpPost("{associatedServiceId}/list-price/{listPriceId}")]
        public async Task<IActionResult> EditListPrice(CatalogueItemId solutionId, CatalogueItemId associatedServiceId, int listPriceId, EditListPriceModel model)
        {
            if (!ModelState.IsValid)
                return View("EditListPrice", model);

            var provisioningType = model.SelectedProvisioningType!.Value;
            var saveListPriceModel = new SaveListPriceModel
            {
                CataloguePriceId = listPriceId,
                Price = model.Price,
                ProvisioningType = provisioningType,
                PricingUnit = model.GetPricingUnit(),
                TimeUnit = model.GetTimeUnit(provisioningType),
            };

            await listPricesService.UpdateListPrice(associatedServiceId, saveListPriceModel);

            return RedirectToAction(nameof(ManageListPrices), new { solutionId, associatedServiceId });
        }

        [HttpGet("{associatedServiceId}/list-price/{listPriceId}/delete")]
        public async Task<IActionResult> DeleteListPrice(CatalogueItemId solutionId, CatalogueItemId associatedServiceId, int listPriceId)
        {
            var associatedService = await listPricesService.GetCatalogueItemWithPrices(associatedServiceId);

            var model = new DeleteListPriceModel(associatedService)
            {
                BackLink = Url.Action(nameof(EditListPrice), new { solutionId, associatedServiceId, listPriceId }),
            };

            return View(model);
        }

        [HttpPost("{associatedServiceId}/list-price/{listPriceId}/delete")]
        public async Task<IActionResult> DeleteListPrice(CatalogueItemId solutionId, CatalogueItemId associatedServiceId, int listPriceId, DeleteListPriceModel model)
        {
            _ = model;

            await listPricesService.DeleteListPrice(associatedServiceId, listPriceId);

            return RedirectToAction(nameof(ManageListPrices), new { solutionId, associatedServiceId });
        }

        [HttpPost("{associatedServiceId}/edit-associated-service")]
        public async Task<IActionResult> SetPublicationStatus(CatalogueItemId solutionId, CatalogueItemId associatedServiceId, EditAssociatedServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                var solution = await solutionsService.GetSolution(solutionId);
                return View("EditAssociatedService", model);
            }

            var associatedService = await associatedServicesService.GetAssociatedService(associatedServiceId);
            if (model.SelectedPublicationStatus == associatedService.PublishedStatus)
                return RedirectToAction(nameof(AssociatedServices), new { solutionId });

            await associatedServicesService.SavePublicationStatus(associatedServiceId, model.SelectedPublicationStatus);

            return RedirectToAction(nameof(AssociatedServices), new { solutionId });
        }
    }
}
