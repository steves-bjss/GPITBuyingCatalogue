﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Addresses.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Models;

namespace NHSD.GPIT.BuyingCatalogue.ServiceContracts.Suppliers
{
    public interface ISuppliersService
    {
        Task<Supplier> AddSupplier(EditSupplierModel model);

        Task<Supplier> EditSupplierDetails(int supplierId, EditSupplierModel updatedSupplier);

        Task<Supplier> GetSupplier(int supplierId);

        Task<Supplier> GetSupplierByName(string supplierName);

        Task<Supplier> GetSupplierByLegalName(string supplierLegalName);

        Task<List<CatalogueItem>> GetAllSolutionsForSupplier(int supplierId);

        Task<IReadOnlyList<Supplier>> GetAllSuppliers();

        Task<IList<Supplier>> GetAllActiveSuppliers();

        Task<Supplier> UpdateSupplierActiveStatus(int supplierId, bool newStatus);

        Task<Supplier> EditSupplierAddress(int supplierId, Address newAddress);

        Task<Supplier> AddSupplierContact(int supplierId, SupplierContact newContact);

        Task<Supplier> EditSupplierContact(int supplierId, int contactId, SupplierContact updatedContact);

        Task<Supplier> DeleteSupplierContact(int supplierId, int contactId);
    }
}