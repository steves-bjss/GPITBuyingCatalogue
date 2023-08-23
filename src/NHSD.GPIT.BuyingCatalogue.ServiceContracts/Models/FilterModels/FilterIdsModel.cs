﻿using System.Collections.Generic;

namespace NHSD.GPIT.BuyingCatalogue.ServiceContracts.Models.FilterModels
{
    public class FilterIdsModel
    {
        public Dictionary<int, string[]> CapabilityAndEpicIds { get; set; }

        public string FrameworkId { get; set; }

        public IEnumerable<int> ApplicationTypeIds { get; set; }

        public IEnumerable<int> HostingTypeIds { get; set; }

        public IEnumerable<string> IM1Integrations { get; set; }

        public IEnumerable<string> GPConnectIntegrations { get; set; }

        public IEnumerable<string> InteroperabilityOptions { get; set; }
    }
}
