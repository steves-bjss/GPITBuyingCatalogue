﻿using System.Collections.Generic;
using System.Linq;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Competitions.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Competitions.Models.Shared;

public class SolutionModel
{
    public SolutionModel()
    {
    }

    public SolutionModel(
        CompetitionSolution competitionSolution)
    {
        SolutionId = competitionSolution.Solution.CatalogueItemId;
        SolutionName = competitionSolution.Solution.CatalogueItem.Name;
        SupplierName = competitionSolution.Solution.CatalogueItem.Supplier.Name;
        RequiredServices = competitionSolution.SolutionServices.Where(x => x.IsRequired).Select(y => y.Service.Name).ToList();
        Selected = competitionSolution.IsShortlisted;
        Summary = competitionSolution.Solution.Summary;
    }

    public CatalogueItemId SolutionId { get; set; }

    public string SolutionName { get; set; }

    public string SupplierName { get; set; }

    public List<string> RequiredServices { get; set; } = new();

    public bool Selected { get; set; }

    public string Summary { get; set; }

    public string GetAdditionalServicesList() => RequiredServices.Any()
        ? string.Join(", ", RequiredServices)
        : "None";
}
