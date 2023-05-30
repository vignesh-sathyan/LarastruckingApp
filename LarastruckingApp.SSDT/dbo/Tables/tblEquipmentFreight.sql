CREATE TABLE [dbo].[tblEquipmentFreight] (
    [MapId]       INT IDENTITY (1, 1) NOT NULL,
    [EquipmentId] INT NOT NULL,
    [FreightId]   INT NOT NULL,
    CONSTRAINT [PK_tblEquipmentFreight_1] PRIMARY KEY CLUSTERED ([MapId] ASC),
    CONSTRAINT [FK_tblEquipmentFreight_tblEquipmentDetail] FOREIGN KEY ([EquipmentId]) REFERENCES [dbo].[tblEquipmentDetail] ([EDID]) ON DELETE CASCADE,
    CONSTRAINT [FK_tblEquipmentFreight_tblFreightType] FOREIGN KEY ([FreightId]) REFERENCES [dbo].[tblFreightType] ([FreightTypeId]) ON DELETE CASCADE
);

