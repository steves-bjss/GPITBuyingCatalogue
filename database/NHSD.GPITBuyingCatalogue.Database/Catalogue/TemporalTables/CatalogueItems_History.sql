﻿CREATE TABLE catalogue.CatalogueItems_History
(
    Id nvarchar(14) NOT NULL,
    [Name] nvarchar(255) NOT NULL,
    CatalogueItemTypeId int NOT NULL,
    SupplierId int NOT NULL,
    PublishedStatusId int NOT NULL,
    Created datetime2(7) NOT NULL,
    LastUpdated datetime2(7) NOT NULL,
    LastUpdatedBy int NULL,
    SysStartTime datetime2(0) NOT NULL,
    SysEndTime datetime2(0) NOT NULL
);
GO

CREATE CLUSTERED COLUMNSTORE INDEX IX_CatalogueItems_History
ON catalogue.CatalogueItems_History;
GO

CREATE NONCLUSTERED INDEX IX_CatalogueItems_History_Id_Period_Columns
ON catalogue.CatalogueItems_History (SysEndTime, SysStartTime, Id);
GO
