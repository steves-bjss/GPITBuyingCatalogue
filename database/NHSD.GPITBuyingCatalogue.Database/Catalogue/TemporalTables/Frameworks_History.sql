﻿CREATE TABLE catalogue.Frameworks_History
(
     Id nvarchar(10) NOT NULL,
     [Name] nvarchar(100) NOT NULL,
     ShortName nvarchar(25) NULL,
     [Description] nvarchar(max) NULL,
     [Owner] nvarchar(100) NULL,
     ActiveDate date NULL,
     ExpiryDate date NULL,
     LastUpdated datetime2(7) NOT NULL,
     LastUpdatedBy int NULL,
     SysStartTime datetime2(0) NOT NULL,
     SysEndTime datetime2(0) NOT NULL
);
GO

CREATE CLUSTERED COLUMNSTORE INDEX IX_Frameworks_History
ON catalogue.Frameworks_History;
GO

CREATE NONCLUSTERED INDEX IX_Frameworks_History_Id_Period_Columns
ON catalogue.Frameworks_History (SysEndTime, SysStartTime, Id);
GO
