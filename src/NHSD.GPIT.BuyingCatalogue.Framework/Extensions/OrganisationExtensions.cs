﻿using Newtonsoft.Json;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.Identity;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Organisations;

namespace NHSD.GPIT.BuyingCatalogue.Framework.Extensions
{
    public static class OrganisationExtensions
    {
        public static Address GetAddress(this Organisation organisation)
        {
            if (string.IsNullOrWhiteSpace(organisation.Address))
                return new Address();

            return JsonConvert.DeserializeObject<Address>(organisation.Address);
        }
    }
}