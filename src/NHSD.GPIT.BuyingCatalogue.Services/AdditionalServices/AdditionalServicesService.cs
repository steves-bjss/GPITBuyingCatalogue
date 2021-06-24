﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NHSD.GPIT.BuyingCatalogue.EntityFramework;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.GPITBuyingCatalogue;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.AdditonalServices;

namespace NHSD.GPIT.BuyingCatalogue.Services.AdditionalServices
{
    public sealed class AdditionalServicesService : IAdditionalServicesService
    {
        private readonly GPITBuyingCatalogueDbContext dbContext;

        public AdditionalServicesService(GPITBuyingCatalogueDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<List<CatalogueItem>> GetAdditionalServicesBySolutionIds(IEnumerable<string> solutionIds)
        {
            return dbContext.CatalogueItems
                .Include(i => i.Solution).ThenInclude(s => s.SolutionCapabilities).ThenInclude(sc => sc.Capability)
                .Include(i => i.Supplier)
                .Include(i => i.AdditionalService)
                .Where(i => solutionIds.Contains(i.AdditionalService.SolutionId)
                    && i.CatalogueItemType == CatalogueItemType.AdditionalService
                    && i.PublishedStatus == PublicationStatus.Published)
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        // TODO: catalogueItemId should be of type CatalogueItemId
        public Task<AdditionalService> GetAdditionalService(string catalogueItemId)
        {
            return dbContext.AdditionalServices.SingleAsync(x => x.CatalogueItemId == catalogueItemId);
        }
    }
}
