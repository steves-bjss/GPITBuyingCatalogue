﻿CREATE TABLE catalogue.AssociatedServices
(
    AssociatedServiceId nvarchar(14) NOT NULL,
    [Description] nvarchar(1000) NULL,
    OrderGuidance nvarchar(1000) NULL,
    LastUpdated datetime2(7) NULL,
    LastUpdatedBy uniqueidentifier NULL,
    CONSTRAINT PK_AssociatedService PRIMARY KEY (AssociatedServiceId),
    CONSTRAINT FK_SupplierService_CatalogueItem FOREIGN KEY (AssociatedServiceId) REFERENCES catalogue.CatalogueItems(CatalogueItemId) ON DELETE CASCADE
);