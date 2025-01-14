﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;

namespace NHSD.GPIT.BuyingCatalogue.ServiceContracts.Integrations;

public interface IIntegrationsService
{
    Task<IEnumerable<Integration>> GetIntegrations();

    Task<IEnumerable<Integration>> GetIntegrationsWithTypes();

    Task<Integration> GetIntegrationWithTypes(SupportedIntegrations integrationId);

    Task<Dictionary<string, IOrderedEnumerable<string>>> GetIntegrationAndTypeNames(
        Dictionary<SupportedIntegrations, int[]> integrationAndTypeIds);

    Task<IEnumerable<IntegrationType>> GetIntegrationTypesByIntegration(SupportedIntegrations integration);

    Task<IntegrationType> GetIntegrationTypeById(SupportedIntegrations integrationId, int integrationTypeId);

    Task<bool> IntegrationTypeExists(SupportedIntegrations integrationId, string integrationTypeName, int? integrationTypeId);

    Task AddIntegrationType(SupportedIntegrations integrationId, string name, string description);

    Task EditIntegrationType(SupportedIntegrations integrationId, int integrationTypeId, string name, string description);
}
