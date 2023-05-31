﻿using System.Collections.Generic;
using System.Linq;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;

namespace NHSD.GPIT.BuyingCatalogue.ServiceContracts.Models.FilterModels;

public class FilterDetailsModel
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string FrameworkName { get; set; }

    public List<HostingType> HostingTypes { get; set; } = Enumerable.Empty<HostingType>().ToList();

    public List<ClientApplicationType> ClientApplicationTypes { get; set; } = Enumerable.Empty<ClientApplicationType>().ToList();

    public List<KeyValuePair<string, List<string>>> Capabilities { get; set; }
}