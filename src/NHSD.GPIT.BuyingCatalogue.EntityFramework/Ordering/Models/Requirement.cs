﻿namespace NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models
{
    public class Requirement
    {
        public int Id { get; set; }

        public int ContractBillingId { get; set; }

        public int OrderId { get; set; }

        public CatalogueItemId CatalogueItemId { get; set; }

        public string Details { get; set; }

        public ContractBilling ContractBilling { get; set; }

        public OrderItem OrderItem { get; set; }
    }
}
