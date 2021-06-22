﻿CREATE PROCEDURE test.RestoreUserAndLogin AS
    SET NOCOUNT ON;

    CREATE LOGIN [NHSD-BAPI]
    WITH PASSWORD = '$(NHSD_PASSWORD)';

    CREATE USER [NHSD-BAPI]
    FOR LOGIN [NHSD-BAPI]
    WITH DEFAULT_SCHEMA = dbo;

    GRANT CONNECT TO [NHSD-BAPI];

    ALTER ROLE Api
    ADD MEMBER [NHSD-BAPI];

    ALTER DATABASE [$(DB_NAME)] SET MULTI_USER;
GO