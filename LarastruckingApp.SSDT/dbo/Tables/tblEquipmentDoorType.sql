CREATE TABLE [dbo].[tblEquipmentDoorType] (
    [MapId]       INT IDENTITY (1, 1) NOT NULL,
    [EquipmentId] INT NOT NULL,
    [DoorTypeId]  INT NOT NULL,
    CONSTRAINT [PK_tblEquipmentDoorType] PRIMARY KEY CLUSTERED ([MapId] ASC),
    CONSTRAINT [FK_tblEquipmentDoorType_tblDoorType] FOREIGN KEY ([DoorTypeId]) REFERENCES [dbo].[tblDoorType] ([DoorTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_tblEquipmentDoorType_tblEquipmentDetail] FOREIGN KEY ([EquipmentId]) REFERENCES [dbo].[tblEquipmentDetail] ([EDID]) ON DELETE CASCADE
);

