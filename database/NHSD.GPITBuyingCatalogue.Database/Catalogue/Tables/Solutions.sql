﻿CREATE TABLE catalogue.Solutions
(
     CatalogueItemId nvarchar(14) NOT NULL,
     Summary nvarchar(350) NULL,
     FullDescription nvarchar(3000) NULL,
     Features nvarchar(max) NULL,
     ClientApplication nvarchar(max) NULL,
     Hosting nvarchar(max) NULL,
     ImplementationDetail nvarchar(1100) NULL,
     RoadMap nvarchar(1000) NULL,
     Integrations nvarchar(max) NULL,
     IntegrationsUrl nvarchar(1000) NULL,
     AboutUrl nvarchar(1000) NULL,
     LastUpdated datetime2(7) NOT NULL,
     LastUpdatedBy int NULL,
     CONSTRAINT PK_Solutions PRIMARY KEY (CatalogueItemId),
     CONSTRAINT FK_Solutions_CatalogueItem FOREIGN KEY (CatalogueItemId) REFERENCES catalogue.CatalogueItems(Id) ON DELETE CASCADE,
     CONSTRAINT FK_Solutions_LastUpdatedBy FOREIGN KEY (LastUpdatedBy) REFERENCES users.AspNetUsers(Id),
);
