CREATE TABLE [dbo].[tblFumigationEquipmentNDriver] (
    [FumigationEquipmentNDriverId] INT IDENTITY (1, 1) NOT NULL,
    [FumigationId]                 INT NULL,
    [FumigationRoutsId]            INT NULL,
    [EquipmentId]                  INT NULL,
    [DriverId]                     INT NULL,
    [IsPickUp]                     BIT DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([FumigationEquipmentNDriverId] ASC),
    FOREIGN KEY ([FumigationId]) REFERENCES [dbo].[tblFumigation] ([FumigationId]),
    FOREIGN KEY ([FumigationRoutsId]) REFERENCES [dbo].[tblFumigationRouts] ([FumigationRoutsId])
);

