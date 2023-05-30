CREATE TYPE [dbo].[UT_FumigationEquipmentNDriver] AS TABLE (
    [FumigationEquipmentNDriverId] INT NULL,
    [RouteNo]                      INT NULL,
    [EquipmentId]                  INT NULL,
    [DriverId]                     INT NULL,
    [IsPickUp]                     BIT NULL,
    [IsDeleted]                    BIT NULL);

