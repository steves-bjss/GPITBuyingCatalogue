﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Competitions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Organisations;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Competitions.Models.DashboardModels;
using NHSD.GPIT.BuyingCatalogue.WebApp.Models;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Competitions.Controllers;

[Authorize("Development")]
[Authorize("Buyer")]
[Area("Competitions")]
[Route("organisation/{internalOrgId}/competitions")]
public class CompetitionsDashboardController : Controller
{
    private readonly IOrganisationsService organisationsService;
    private readonly ICompetitionsService competitionsService;
    private readonly IManageFiltersService filterService;

    public CompetitionsDashboardController(
        IOrganisationsService organisationsService,
        ICompetitionsService competitionsService,
        IManageFiltersService filterService)
    {
        this.organisationsService =
            organisationsService ?? throw new ArgumentNullException(nameof(organisationsService));
        this.competitionsService = competitionsService ?? throw new ArgumentNullException(nameof(competitionsService));
        this.filterService = filterService ?? throw new ArgumentNullException(nameof(filterService));
    }

    [HttpGet]
    public async Task<IActionResult> Index(string internalOrgId)
    {
        var organisation = await organisationsService.GetOrganisationByInternalIdentifier(internalOrgId);

        var competitions = await competitionsService.GetCompetitions(organisation.Id);

        var model = new CompetitionDashboardModel(internalOrgId, organisation.Name, competitions.ToList());

        return View(model);
    }

    [HttpGet("before-you-start")]
    public IActionResult BeforeYouStart(string internalOrgId)
    {
        var model = new NavBaseModel { BackLink = Url.Action(nameof(Index), new { internalOrgId }) };

        return View(model);
    }

    [HttpPost("before-you-start")]
    public IActionResult BeforeYouStart(string internalOrgId, NavBaseModel model)
    {
        _ = model;

        return RedirectToAction(nameof(SelectFilter), new { internalOrgId });
    }

    [HttpGet("select-filter")]
    public async Task<IActionResult> SelectFilter(string internalOrgId)
    {
        var organisation = await organisationsService.GetOrganisationByInternalIdentifier(internalOrgId);
        var filters = await filterService.GetFilters(organisation.Id);

        var model = new SelectFilterModel(organisation.Name, filters)
        {
            BackLink = Url.Action(nameof(BeforeYouStart), new { internalOrgId }),
        };

        return View(model);
    }

    [HttpPost("select-filter")]
    public async Task<IActionResult> SelectFilter(string internalOrgId, SelectFilterModel model)
    {
        if (ModelState.IsValid)
            return RedirectToAction(nameof(ReviewFilter), new { internalOrgId, filterId = model.SelectedFilterId });

        var organisation = await organisationsService.GetOrganisationByInternalIdentifier(internalOrgId);
        var filters = await filterService.GetFilters(organisation.Id);

        model.WithFilters(filters);
        return View(model);
    }

    [HttpGet("select-filter/{filterId}/review")]
    public async Task<IActionResult> ReviewFilter(string internalOrgId, int filterId)
    {
        var organisation = await organisationsService.GetOrganisationByInternalIdentifier(internalOrgId);
        var filterDetails = await filterService.GetFilterDetails(organisation.Id, filterId);

        if (filterDetails == null)
            return RedirectToAction(nameof(SelectFilter), new { internalOrgId });

        var model = new ReviewFilterModel(filterDetails)
        {
            BackLink = Url.Action(nameof(SelectFilter), new { internalOrgId }),
        };

        return View(model);
    }

    [HttpPost("select-filter/{filterId}/review")]
    public IActionResult ReviewFilter(string internalOrgId, int filterId, ReviewFilterModel model)
    {
        _ = model;

        return RedirectToAction(nameof(SaveCompetition), new { internalOrgId, filterId });
    }

    [HttpGet("select-filter/{filterId}/save")]
    public async Task<IActionResult> SaveCompetition(string internalOrgId, int filterId)
    {
        _ = filterId;
        var organisation = await organisationsService.GetOrganisationByInternalIdentifier(internalOrgId);

        var model = new SaveCompetitionModel(organisation.Id, organisation.Name)
        {
            BackLink = Url.Action(nameof(ReviewFilter), new { internalOrgId, filterId }),
        };

        return View(model);
    }

    [HttpPost("select-filter/{filterId}/save")]
    public async Task<IActionResult> SaveCompetition(string internalOrgId, int filterId, SaveCompetitionModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var organisation = await organisationsService.GetOrganisationByInternalIdentifier(internalOrgId);

        await competitionsService.AddCompetition(organisation.Id, filterId, model.Name, model.Description);

        return RedirectToAction(nameof(Index), new { internalOrgId });
    }
}