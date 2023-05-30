CREATE TABLE [dbo].[tblShipmentEquipmentNdriver] (
    [ShipmentEquipmentNdriverId] INT      IDENTITY (1, 1) NOT NULL,
    [ShipmentId]                 INT      NULL,
    [DriverId]                   INT      NULL,
    [EquipmentId]                INT      NULL,
    [IsActive]                   BIT      DEFAULT ((1)) NOT NULL,
    [CreatedDate]                DATETIME DEFAULT (getutcdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([ShipmentEquipmentNdriverId] ASC),
    FOREIGN KEY ([DriverId]) REFERENCES [dbo].[tblDriver] ([DriverID]),
    FOREIGN KEY ([EquipmentId]) REFERENCES [dbo].[tblEquipmentDetail] ([EDID]),
    CONSTRAINT [FK__tblShipme__Shipm__382F5661] FOREIGN KEY ([ShipmentId]) REFERENCES [dbo].[tblShipment] ([ShipmentId])
);






GO
CREATE NONCLUSTERED INDEX [ix_tblShipmentEquipmentNdriver]
    ON [dbo].[tblShipmentEquipmentNdriver]([ShipmentId] ASC);

