﻿CREATE TABLE ordering.OrderItemRecipients_History
(
    OrderId int NOT NULL,
    CatalogueItemId nvarchar(14) NOT NULL,
    OdsCode nvarchar(8) NOT NULL,
    Quantity int NOT NULL,
    DeliveryDate date NULL,
    LastUpdated datetime2(7) NOT NULL,
    LastUpdatedBy int NULL,
    SysStartTime datetime2(0) NOT NULL,
    SysEndTime datetime2(0) NOT NULL
);
GO

CREATE CLUSTERED COLUMNSTORE INDEX IX_OrderItemRecipients_History
ON ordering.OrderItemRecipients_History;
GO

CREATE NONCLUSTERED INDEX IX_OrderItemRecipients_History_OrderId_CatalogueItemId_OdsCode_Period_Columns
ON ordering.OrderItemRecipients_History (SysEndTime, SysStartTime, OrderId, CatalogueItemId, OdsCode);
GO
