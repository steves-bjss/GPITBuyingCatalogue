﻿using System.Collections.Generic;

namespace NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models
{
    public class IntegrationSubType
    {
        public string Name { get; set; }

        public Dictionary<string, string> DetailsDictionary { get; set; } = new();

        public Dictionary<string, Dictionary<string, string>> DetailsSystemDictionary { get; set; } = new();
    }
}