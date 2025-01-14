﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EnumsNET;
using Microsoft.EntityFrameworkCore;
using NHSD.GPIT.BuyingCatalogue.EntityFramework;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Enums;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions.Admin;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions.Models;

namespace NHSD.GPIT.BuyingCatalogue.Services.Solutions
{
    public sealed class SolutionsService : ISolutionsService
    {
        private readonly BuyingCatalogueDbContext dbContext;

        public SolutionsService(BuyingCatalogueDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<CatalogueItem> GetSolutionThin(CatalogueItemId solutionId) =>
            dbContext.CatalogueItems.AsNoTracking()
            .Include(ci => ci.Supplier)
            .Include(ci => ci.Solution)
            .ThenInclude(x => x.Integrations)
            .ThenInclude(x => x.IntegrationType)
            .ThenInclude(x => x.Integration)
            .AsSplitQuery()
            .FirstOrDefaultAsync(ci => ci.Id == solutionId);

        public async Task<CatalogueItem> GetSolutionWithBasicInformation(CatalogueItemId solutionId) =>
            await dbContext.CatalogueItems.AsNoTracking()
                .Include(ci => ci.Solution)
                    .ThenInclude(s => s.FrameworkSolutions)
                    .ThenInclude(s => s.Framework)
                .Include(ci => ci.Solution)
                .ThenInclude(x => x.Integrations)
                .ThenInclude(x => x.IntegrationType)
                .ThenInclude(x => x.Integration)
                .Include(ci => ci.Supplier)
                .FirstOrDefaultAsync(ci => ci.Id == solutionId);

        public async Task<CatalogueItem> GetSolutionWithCapabilities(CatalogueItemId solutionId) =>
            await dbContext.CatalogueItems.AsNoTracking()
                .Include(ci => ci.Solution)
                    .ThenInclude(s => s.FrameworkSolutions)
                    .ThenInclude(s => s.Framework)
                .Include(ci => ci.Supplier)
                .Include(ci => ci.CatalogueItemCapabilities)
                    .ThenInclude(cic => cic.Capability)
                .Include(ci => ci.CatalogueItemEpics)
                    .ThenInclude(cie => cie.Epic)
                .FirstOrDefaultAsync(ci => ci.Id == solutionId);

        public async Task<CatalogueItem> GetSolutionWithServiceLevelAgreements(CatalogueItemId solutionId) =>
            await dbContext.CatalogueItems.AsNoTracking()
                .Include(ci => ci.Supplier)
                .Include(ci => ci.Solution).ThenInclude(s => s.ServiceLevelAgreement).ThenInclude(sla => sla.Contacts)
                .Include(ci => ci.Solution).ThenInclude(s => s.ServiceLevelAgreement).ThenInclude(sla => sla.ServiceHours)
                .Include(ci => ci.Solution).ThenInclude(s => s.ServiceLevelAgreement).ThenInclude(sla => sla.ServiceLevels)
                .Include(ci => ci.Solution)
                    .ThenInclude(s => s.FrameworkSolutions)
                    .ThenInclude(s => s.Framework)
            .FirstOrDefaultAsync(ci => ci.Id == solutionId);

        public async Task<CatalogueItem> GetSolutionWithCataloguePrice(CatalogueItemId solutionId) =>
            await dbContext.CatalogueItems.AsNoTracking()
                .Include(ci => ci.CataloguePrices)
                .ThenInclude(p => p.CataloguePriceTiers)
                .Include(ci => ci.CataloguePrices)
                .ThenInclude(p => p.PricingUnit)
                .Include(ci => ci.Solution)
                    .ThenInclude(s => s.FrameworkSolutions)
                    .ThenInclude(s => s.Framework)
                .Include(ci => ci.Supplier)
                .FirstOrDefaultAsync(ci => ci.Id == solutionId);

        public async Task<CatalogueItem> GetSolutionWithDataProcessingInformation(CatalogueItemId solutionId) =>
            await dbContext
            .CatalogueItems
            .Include(x => x.Solution)
            .Include(x => x.Solution.DataProcessingInformation)
            .Include(x => x.Solution.DataProcessingInformation.Details)
            .Include(x => x.Solution.DataProcessingInformation.Location)
            .Include(x => x.Solution.DataProcessingInformation.Officer)
            .Include(x => x.Solution.DataProcessingInformation.SubProcessors)
            .ThenInclude(x => x.Details)
            .Include(x => x.Solution.FrameworkSolutions).ThenInclude(x => x.Framework)
            .Include(x => x.Supplier)
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == solutionId);

        public async Task<CatalogueItem> GetSolutionWithSupplierDetails(CatalogueItemId solutionId) =>
            await dbContext.CatalogueItems.AsNoTracking()
                .Include(ci => ci.Solution)
                    .ThenInclude(s => s.FrameworkSolutions)
                    .ThenInclude(s => s.Framework)
                .Include(ci => ci.Supplier)
                    .ThenInclude(s => s.SupplierContacts)
                .Include(ci => ci.CatalogueItemContacts)
                .FirstOrDefaultAsync(ci => ci.Id == solutionId);

        public async Task<CatalogueItem> GetSolutionWithWorkOffPlans(CatalogueItemId solutionId) =>
            await dbContext.CatalogueItems.AsNoTracking()
                .Include(ci => ci.Solution)
                    .ThenInclude(s => s.WorkOffPlans)
                    .ThenInclude(wp => wp.Standard)
                .FirstOrDefaultAsync(ci => ci.Id == solutionId);

        public async Task<CatalogueItem> GetSolutionWithServiceAssociations(CatalogueItemId solutionId) =>
            await dbContext.CatalogueItems.AsNoTracking()
            .Include(ci => ci.Supplier)
            .Include(ci => ci.Solution)
            .Include(ci => ci.SupplierServiceAssociations)
            .FirstOrDefaultAsync(ci => ci.Id == solutionId);

        public async Task<SolutionLoadingStatusesModel> GetSolutionLoadingStatuses(CatalogueItemId solutionId)
        {
            var solution = await dbContext.CatalogueItems
                .AsNoTracking()
                .Where(ci => ci.Id == solutionId)
                .Select(
                    ci => new SolutionLoadingStatusesModel
                    {
                        Description = (!string.IsNullOrWhiteSpace(ci.Solution.Summary))
                            ? TaskProgress.Completed
                            : TaskProgress.NotStarted,
                        AdditionalServices = ci.Solution.AdditionalServices
                            .Any(add => add.CatalogueItem.PublishedStatus == PublicationStatus.Published)
                            ? TaskProgress.Completed
                            : ci.Solution.AdditionalServices.Any()
                                ? TaskProgress.InProgress
                                : TaskProgress.Optional,
                        AssociatedServices = ci.SupplierServiceAssociations.Any()
                            ? TaskProgress.Completed
                            : TaskProgress.Optional,
                        Features = !string.IsNullOrWhiteSpace(ci.Solution.Features)
                            ? TaskProgress.Completed
                            : TaskProgress.Optional,
                        Interoperability = ci.Solution.Integrations.Count > 0
                            ? TaskProgress.Completed
                            : TaskProgress.Optional,
                        Implementation = !string.IsNullOrWhiteSpace(ci.Solution.ImplementationDetail)
                            ? TaskProgress.Completed
                            : TaskProgress.Optional,
                        ListPrice = ci.CataloguePrices.Any(cp => cp.PublishedStatus == PublicationStatus.Published)
                            ? TaskProgress.Completed
                            : ci.CataloguePrices.Any()
                                ? TaskProgress.InProgress
                                : TaskProgress.NotStarted,
                        ApplicationType = ci.Solution.ApplicationTypeDetail != null
                            ? TaskProgress.Completed
                            : TaskProgress.NotStarted,
                        HostingType = ci.Solution.Hosting != null && ci.Solution.Hosting.IsValid()
                            ? TaskProgress.Completed
                            : TaskProgress.NotStarted,
                        DataProcessing = ci.Solution.DataProcessingInformation != null
                            && ci.Solution.DataProcessingInformation.Details != null
                            && ci.Solution.DataProcessingInformation.Location != null
                            && ci.Solution.DataProcessingInformation.Officer != null
                                ? TaskProgress.Completed
                                : ci.Solution.DataProcessingInformation != null
                                && (ci.Solution.DataProcessingInformation.Details != null
                                    || ci.Solution.DataProcessingInformation.Location != null
                                    || ci.Solution.DataProcessingInformation.Officer != null
                                    || ci.Solution.DataProcessingInformation.SubProcessors.Any())
                                    ? TaskProgress.InProgress
                                    : TaskProgress.NotStarted,
                        DevelopmentPlans = ci.Solution.WorkOffPlans.Any()
                            ? TaskProgress.Completed
                            : TaskProgress.Optional,
                        CapabilitiesAndEpics = ci.CatalogueItemCapabilities.Any()
                            ? TaskProgress.Completed
                            : TaskProgress.NotStarted,
                        SupplierDetails = ci.CatalogueItemContacts.Any()
                            ? TaskProgress.Completed
                            : TaskProgress.NotStarted,
                        ServiceLevelAgreement = (ci.Solution.ServiceLevelAgreement != null &&
                            ci.Solution.ServiceLevelAgreement.Contacts.Any() &&
                            ci.Solution.ServiceLevelAgreement.ServiceHours.Any() &&
                            ci.Solution.ServiceLevelAgreement.ServiceLevels.Any())
                            ? TaskProgress.Completed
                            : (ci.Solution.ServiceLevelAgreement.Contacts.Any() ||
                                ci.Solution.ServiceLevelAgreement.ServiceHours.Any() ||
                                ci.Solution.ServiceLevelAgreement.ServiceLevels.Any())
                                ? TaskProgress.InProgress
                                : TaskProgress.NotStarted,
                    })
                .FirstOrDefaultAsync();

            return solution;
        }

        public Task<CatalogueItem> GetSolutionByName(string solutionName)
        {
            return dbContext.CatalogueItems
                .Include(i => i.Solution)
                .Include(i => i.CatalogueItemCapabilities).ThenInclude(cic => cic.Capability)
                .Include(i => i.Supplier)
                .Include(i => i.Solution).ThenInclude(s => s.FrameworkSolutions).ThenInclude(fs => fs.Framework)
                .Include(i => i.Solution).ThenInclude(s => s.MarketingContacts)
                .Where(i => i.Name == solutionName)
                .FirstOrDefaultAsync();
        }

        public Task<bool> CatalogueSolutionExistsWithName(string solutionName, CatalogueItemId currentCatalogueItemId = default) =>
            dbContext
                .CatalogueItems
                .AnyAsync(ci =>
                ci.CatalogueItemType == CatalogueItemType.Solution
                && ci.Name == solutionName
                && ci.Id != currentCatalogueItemId);

        public async Task<CatalogueItem> GetSolutionCapability(CatalogueItemId catalogueItemId, int capabilityId)
        {
            return await dbContext.CatalogueItems.AsNoTracking()
                .Include(ci => ci.Solution)
                .Include(ci => ci.CatalogueItemCapabilities.Where(c => c.CapabilityId == capabilityId)).ThenInclude(cic => cic.Capability)
                .Include(ci => ci.CatalogueItemEpics.Where(cie => cie.CapabilityId == capabilityId && cie.Epic.IsActive)).ThenInclude(cie => cie.Epic)
                .FirstOrDefaultAsync(c => c.Id == catalogueItemId);
        }

        public async Task<IList<Standard>> GetSolutionStandardsForMarketing(CatalogueItemId catalogueItemId)
        {
            var requiredStandardsQuery = dbContext.Standards.Where(s => s.StandardType == StandardType.Overarching);

            var capabilityStandards = dbContext.WorkOffPlans.Where(wp => wp.SolutionId == catalogueItemId).Select(wp => wp.Standard).Distinct();

            return await dbContext.CatalogueItems
               .Where(ci => ci.Id == catalogueItemId)
               .SelectMany(ci => ci.CatalogueItemCapabilities)
               .Select(cic => cic.Capability)
               .SelectMany(c => c.StandardCapabilities)
               .Select(sc => sc.Standard)
               .Distinct()
               .Union(requiredStandardsQuery)
               .Union(capabilityStandards)
               .ToListAsync();
        }

        public async Task<IList<Standard>> GetSolutionStandardsForEditing(CatalogueItemId catalogueItemId)
        {
            return await dbContext.CatalogueItems
               .Where(ci => ci.Id == catalogueItemId)
               .SelectMany(ci => ci.CatalogueItemCapabilities)
               .Select(cic => cic.Capability)
               .SelectMany(c => c.StandardCapabilities)
               .Select(sc => sc.Standard)
               .Distinct()
               .Union(dbContext.Standards)
               .ToListAsync();
        }

        public async Task<CatalogueItemContentStatus> GetContentStatusForCatalogueItem(CatalogueItemId solutionId) =>
            await dbContext.CatalogueItems.AsNoTracking()
                .Where(ci => ci.Id == solutionId)
                .Select(ci => new CatalogueItemContentStatus
                {
                    ShowFeatures = !string.IsNullOrWhiteSpace(ci.Solution.Features),
                    ShowAdditionalServices = ci.Solution.AdditionalServices.Any(add =>
                        add.CatalogueItem.PublishedStatus == PublicationStatus.Published),
                    ShowAssociatedServices = ci.SupplierServiceAssociations.Any(ssa =>
                        ssa.AssociatedService.CatalogueItem.PublishedStatus == PublicationStatus.Published),
                    ShowInteroperability = ci.Solution.Integrations.Count > 0 || !string.IsNullOrWhiteSpace(ci.Solution.IntegrationsUrl),
                    ShowImplementation = !string.IsNullOrWhiteSpace(ci.Solution.ImplementationDetail),
                    ShowHosting = ci.Solution.Hosting != null && ci.Solution.Hosting.IsValid(),
                }).FirstOrDefaultAsync();

        public async Task<List<CatalogueItem>> GetPublishedAdditionalServicesForSolution(CatalogueItemId solutionId) =>
            await dbContext.CatalogueItems
                .Include(ci => ci.AdditionalService)
                .Include(ci => ci.CataloguePrices.Where(cp => cp.PublishedStatus == PublicationStatus.Published))
                .ThenInclude(p => p.CataloguePriceTiers)
                .Include(ci => ci.CataloguePrices.Where(cp => cp.PublishedStatus == PublicationStatus.Published))
                .ThenInclude(p => p.PricingUnit)
                .Where(ci =>
                ci.AdditionalService.SolutionId == solutionId
                && ci.PublishedStatus == PublicationStatus.Published)
                .ToListAsync();

        public async Task<List<CatalogueItem>> GetPublishedAssociatedServicesForSolution(CatalogueItemId solutionId)
        {
            var selectedAssociatedServices = dbContext.CatalogueItems
                .Where(ci => ci.Id == solutionId)
                .SelectMany(ci => ci.SupplierServiceAssociations);

            return await dbContext.CatalogueItems
                    .Include(ci => ci.AssociatedService)
                    .Include(ci => ci.CataloguePrices.Where(cp => cp.PublishedStatus == PublicationStatus.Published))
                    .ThenInclude(p => p.CataloguePriceTiers)
                    .Include(ci => ci.CataloguePrices.Where(cp => cp.PublishedStatus == PublicationStatus.Published))
                    .ThenInclude(p => p.PricingUnit)
                    .Where(ci =>
                        ci.PublishedStatus == PublicationStatus.Published
                        && selectedAssociatedServices.Any(sas => sas.AssociatedServiceId == ci.Id))
                    .ToListAsync();
        }

        public async Task SaveSolutionDetails(
            CatalogueItemId id,
            string solutionName,
            int supplierId,
            bool isPilotSolution,
            IList<FrameworkModel> selectedFrameworks)
        {
            var data = await GetCatalogueItem(id);

            data.Name = solutionName;
            data.SupplierId = supplierId;
            data.Solution.IsPilotSolution = isPilotSolution;

            var frameworks = data.Solution.FrameworkSolutions.ToList();
            frameworks.RemoveAll(f => selectedFrameworks.Any(sf => f.FrameworkId == sf.FrameworkId && sf.Selected == false));

            foreach (var framework in selectedFrameworks.Where(f => f.Selected))
            {
                var existingFramework = frameworks.FirstOrDefault(fs => fs.FrameworkId == framework.FrameworkId);

                if (existingFramework is null)
                {
                    frameworks.Add(new FrameworkSolution
                    {
                        FrameworkId = framework.FrameworkId,
                    });
                }
            }

            data.Solution.FrameworkSolutions = frameworks;
            await dbContext.SaveChangesAsync();
        }

        public async Task SaveSolutionDescription(CatalogueItemId solutionId, string summary, string description, string link)
        {
            summary.ValidateNotNullOrWhiteSpace(nameof(summary));

            var solution = await dbContext.Solutions.FirstAsync(s => s.CatalogueItemId == solutionId);
            solution.Summary = summary;
            solution.FullDescription = description;
            solution.AboutUrl = link;
            await dbContext.SaveChangesAsync();
        }

        public async Task SaveSolutionFeatures(CatalogueItemId solutionId, string[] features)
        {
            var solution = await dbContext.Solutions.FirstAsync(s => s.CatalogueItemId == solutionId);
            solution.Features = JsonSerializer.Serialize(features);
            await dbContext.SaveChangesAsync();
        }

        public async Task SaveImplementationDetail(CatalogueItemId solutionId, string detail)
        {
            var solution = await dbContext.Solutions.FirstAsync(s => s.CatalogueItemId == solutionId);
            solution.ImplementationDetail = detail;
            await dbContext.SaveChangesAsync();
        }

        public async Task<ApplicationTypeDetail> GetApplicationType(CatalogueItemId solutionId)
        {
            var solution = await dbContext.Solutions.FirstAsync(s => s.CatalogueItemId == solutionId);
            return solution.EnsureAndGetApplicationType();
        }

        public async Task SaveApplicationType(CatalogueItemId solutionId, ApplicationTypeDetail applicationTypeDetail)
        {
            applicationTypeDetail.ValidateNotNull(nameof(applicationTypeDetail));

            var solution = await dbContext.Solutions.FirstAsync(s => s.CatalogueItemId == solutionId);
            solution.ApplicationTypeDetail = applicationTypeDetail;
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteApplicationType(CatalogueItemId solutionId, ApplicationType applicationType)
        {
            var applicationTypeDetail = await GetApplicationType(solutionId);

            RemoveApplicationType(applicationTypeDetail, applicationType);

            await SaveApplicationType(solutionId, applicationTypeDetail);
        }

        public async Task SaveHosting(CatalogueItemId solutionId, Hosting hosting)
        {
            ArgumentNullException.ThrowIfNull(hosting);

            var solution = await dbContext.Solutions.FirstAsync(s => s.CatalogueItemId == solutionId);
            solution.Hosting = hosting;
            await dbContext.SaveChangesAsync();
        }

        public async Task SaveSupplierDescriptionAndLink(int supplierId, string description, string link)
        {
            var supplier = await dbContext.Suppliers.FirstAsync(s => s.Id == supplierId);
            supplier.Summary = description;
            supplier.SupplierUrl = link;
            await dbContext.SaveChangesAsync();
        }

        public async Task SaveSupplierContacts(SupplierContactsModel model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            model.SetSolutionId();

            var marketingContacts = await dbContext.MarketingContacts.Where(c => c.SolutionId == model.SolutionId).ToListAsync();

            if (!marketingContacts.Any())
            {
                dbContext.MarketingContacts.AddRange(model.ValidContacts());
            }
            else
            {
                foreach (var contact in marketingContacts)
                {
                    if (model.ContactFor(contact.Id) is not { } newContact)
                        continue;

                    if (newContact.IsEmpty())
                        dbContext.MarketingContacts.Remove(contact);
                    else
                        contact.UpdateFrom(newContact);
                }

                dbContext.MarketingContacts.AddRange(model.NewAndValidContacts());
            }

            await dbContext.SaveChangesAsync();
        }

        public Task<List<CatalogueItem>> GetSupplierSolutions(int? supplierId, string selectedFrameworkId)
        {
            var query = dbContext.CatalogueItems
                .Include(i => i.Solution)
                .Include(i => i.CatalogueItemCapabilities).ThenInclude(sc => sc.Capability)
                .Include(i => i.Supplier)
                .Where(i => i.SupplierId == supplierId.GetValueOrDefault()
                    && i.CatalogueItemType == CatalogueItemType.Solution
                    && i.PublishedStatus == PublicationStatus.Published);

            if (!string.IsNullOrEmpty(selectedFrameworkId))
            {
                query = query
                    .Where(i => i.Solution.FrameworkSolutions.Any(x => !x.Framework.IsExpired && x.FrameworkId == selectedFrameworkId));
            }
            else
            {
                query = query
                    .Where(i => i.Solution.FrameworkSolutions.Select(x => x.Framework).Distinct().Any(x => !x.IsExpired));
            }

            return query
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        public async Task<List<CatalogueItem>> GetSupplierSolutionsWithAssociatedServices(int? supplierId, PracticeReorganisationTypeEnum practiceReorganisationType, string selectedFrameworkId)
        {
            var query = dbContext.CatalogueItems.AsNoTracking()
                .Include(x => x.SupplierServiceAssociations).ThenInclude(x => x.AssociatedService).ThenInclude(x => x.CatalogueItem)
                .Include(i => i.Solution)
                .Include(i => i.CatalogueItemCapabilities).ThenInclude(sc => sc.Capability)
                .Include(i => i.Supplier)
                .Where(i => i.SupplierId == supplierId.GetValueOrDefault()
                    && i.CatalogueItemType == CatalogueItemType.Solution
                    && i.PublishedStatus == PublicationStatus.Published
                    && i.SupplierServiceAssociations != null);

            if (!string.IsNullOrEmpty(selectedFrameworkId))
            {
                query = query
                    .Where(i => i.Solution.FrameworkSolutions.Any(x => !x.Framework.IsExpired && x.FrameworkId == selectedFrameworkId));
            }
            else
            {
                query = query
                    .Where(i => i.Solution.FrameworkSolutions.Select(x => x.Framework).Distinct().Any(x => !x.IsExpired));
            }

            if (practiceReorganisationType != PracticeReorganisationTypeEnum.None)
            {
                query = query
                    .Where(i => i.SupplierServiceAssociations.Any(x => x.AssociatedService != null
                        && (x.AssociatedService.PracticeReorganisationType & practiceReorganisationType) == practiceReorganisationType
                        && x.AssociatedService.CatalogueItem.PublishedStatus == PublicationStatus.Published));
            }
            else
            {
                query = query
                    .Where(i => i.SupplierServiceAssociations.Any(x => x.AssociatedService != null
                        && x.AssociatedService.CatalogueItem.PublishedStatus == PublicationStatus.Published));
            }

            return await query
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        public async Task<IList<CatalogueItem>> GetAllSolutions(
            PublicationStatus? publicationStatus = null)
        {
            var query = dbContext.CatalogueItems
                .Include(i => i.Solution)
                .Include(i => i.Supplier)
                .Include(i => i.CatalogueItemCapabilities).ThenInclude(cic => cic.Capability)
                .Where(i => i.CatalogueItemType == CatalogueItemType.Solution);

            if (publicationStatus is not null)
                query = query.Where(i => i.PublishedStatus == publicationStatus.Value);

            return await query
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        public async Task<IList<CatalogueItem>> GetAllSolutionsForSearchTerm(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentNullException(nameof(searchTerm));

            return await dbContext.CatalogueItems
                .Include(i => i.Solution)
                .Include(i => i.Supplier)
                .Include(i => i.CatalogueItemCapabilities).ThenInclude(cic => cic.Capability)
                .Where(i => i.CatalogueItemType == CatalogueItemType.Solution
                    && (EF.Functions.Like(i.Name, $"%{searchTerm}%")
                        || EF.Functions.Like(i.Supplier.Name, $"%{searchTerm}%")))
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        public async Task<CatalogueItemId> AddCatalogueSolution(CreateSolutionModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            model.Frameworks.ValidateNotNull(nameof(CreateSolutionModel.Frameworks));

            var dateTimeNow = DateTime.UtcNow;

            var frameworkSolutions = new List<FrameworkSolution>();

            foreach (var framework in model.Frameworks.Where(f => f.Selected))
            {
                frameworkSolutions.Add(new FrameworkSolution
                {
                    FrameworkId = framework.FrameworkId,
                });
            }

            var catalogueItem = new CatalogueItem
            {
                CatalogueItemType = CatalogueItemType.Solution,
                Solution =
                        new Solution
                        {
                            FrameworkSolutions = frameworkSolutions,
                            IsPilotSolution = model.IsPilotSolution,
                        },
                Name = model.Name,
                PublishedStatus = PublicationStatus.Draft,
                SupplierId = model.SupplierId,
            };

            dbContext.CatalogueItems.Add(catalogueItem);

            await dbContext.SaveChangesAsync();

            return catalogueItem.Id;
        }

        public async Task<IList<EntityFramework.Catalogue.Models.Framework>> GetAllFrameworks()
        {
            return await dbContext.Frameworks.ToListAsync();
        }

        public Task<bool> SupplierHasSolutionName(int supplierId, string solutionName) =>
            dbContext.CatalogueItems.AnyAsync(i => i.SupplierId == supplierId && i.Name == solutionName);

        public async Task SaveContacts(CatalogueItemId solutionId, IList<SupplierContact> supplierContacts)
        {
            var solution = await dbContext.CatalogueItems.Include(i => i.CatalogueItemContacts).FirstAsync(i => i.Id == solutionId);

            var staleContacts = solution
                .CatalogueItemContacts
                .Where(c => !supplierContacts.Any(sc =>
                           sc.Id == c.Id
                        && sc.FirstName.EqualsIgnoreCase(c.FirstName)
                        && sc.LastName.EqualsIgnoreCase(c.LastName)
                        && sc.SupplierId == c.SupplierId));

            var newContacts = supplierContacts.Where(sc => !solution.CatalogueItemContacts.Any(c =>
                           sc.Id == c.Id
                        && sc.FirstName.EqualsIgnoreCase(c.FirstName)
                        && sc.LastName.EqualsIgnoreCase(c.LastName)
                        && sc.SupplierId == c.SupplierId));

            foreach (var staleContact in staleContacts.ToList())
            {
                solution.CatalogueItemContacts.Remove(staleContact);
            }

            solution.CatalogueItemContacts.AddRange(newContacts);

            await dbContext.SaveChangesAsync();
        }

        public async Task<List<WorkOffPlan>> GetWorkOffPlans(CatalogueItemId solutionId) =>
            await dbContext.WorkOffPlans
                .Include(wp => wp.Standard)
                .Where(wp => wp.SolutionId == solutionId).ToListAsync();

        internal static ApplicationTypeDetail RemoveApplicationType(ApplicationTypeDetail applicationTypeDetail, ApplicationType applicationType)
        {
            if (applicationTypeDetail is null)
                throw new ArgumentNullException(nameof(applicationTypeDetail));

            if (applicationTypeDetail.ApplicationTypes is not null)
            {
                if (applicationTypeDetail.ApplicationTypes.Contains(applicationType.AsString(EnumFormat.EnumMemberValue)))
                    applicationTypeDetail.ApplicationTypes.Remove(applicationType.AsString(EnumFormat.EnumMemberValue));
            }

            if (applicationType == ApplicationType.BrowserBased)
            {
                applicationTypeDetail.AdditionalInformation = null;
                applicationTypeDetail.BrowsersSupported = null;
                applicationTypeDetail.HardwareRequirements = null;
                applicationTypeDetail.MinimumConnectionSpeed = null;
                applicationTypeDetail.MinimumDesktopResolution = null;
                applicationTypeDetail.MobileFirstDesign = null;
                applicationTypeDetail.MobileResponsive = null;
                applicationTypeDetail.Plugins = null;
            }
            else if (applicationType == ApplicationType.Desktop)
            {
                applicationTypeDetail.NativeDesktopAdditionalInformation = null;
                applicationTypeDetail.NativeDesktopHardwareRequirements = null;
                applicationTypeDetail.NativeDesktopMemoryAndStorage = null;
                applicationTypeDetail.NativeDesktopMinimumConnectionSpeed = null;
                applicationTypeDetail.NativeDesktopOperatingSystemsDescription = null;
                applicationTypeDetail.NativeDesktopThirdParty = null;
            }
            else
            {
                applicationTypeDetail.MobileConnectionDetails = null;
                applicationTypeDetail.MobileMemoryAndStorage = null;
                applicationTypeDetail.MobileOperatingSystems = null;
                applicationTypeDetail.MobileThirdParty = null;
                applicationTypeDetail.NativeMobileAdditionalInformation = null;
                applicationTypeDetail.NativeMobileFirstDesign = null;
                applicationTypeDetail.NativeMobileHardwareRequirements = null;
            }

            return applicationTypeDetail;
        }

        private async Task<CatalogueItem> GetCatalogueItem(CatalogueItemId id) => await dbContext.CatalogueItems
            .Include(s => s.Solution)
            .Include(s => s.Solution.FrameworkSolutions)
            .FirstAsync(s => s.Id == id);
    }
}
