﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.Identity;

namespace NHSD.GPIT.BuyingCatalogue.ServiceContracts.Organisations
{
    public interface IOrganisationsService
    {
        Task<List<Organisation>> GetAllOrganisations();

        Task<Organisation> GetOrganisation(Guid id);

        Task<Guid> AddOdsOrganisation(OdsOrganisation odsOrganisation, bool agreementSigned);

        Task UpdateCatalogueAgreementSigned(Guid organisationId, bool signed);

        Task<List<Organisation>> GetUnrelatedOrganisations(Guid organisationId);

        Task<List<Organisation>> GetRelatedOrganisations(Guid organisationId);

        Task AddRelatedOrganisations(Guid organisationId, Guid relatedOrganisationId);

        Task RemoveRelatedOrganisations(Guid organisationId, Guid relatedOrganisationId);
    }
}