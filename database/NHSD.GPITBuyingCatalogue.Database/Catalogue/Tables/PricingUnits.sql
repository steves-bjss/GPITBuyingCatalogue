﻿CREATE TABLE catalogue.PricingUnits
(
    Id uniqueidentifier NOT NULL,
    [Name] nvarchar(20) NOT NULL,
    TierName nvarchar(30) NOT NULL,
    [Description] nvarchar(40) NOT NULL,
    CONSTRAINT PK_PricingUnit PRIMARY KEY NONClUSTERED (Id),
);
