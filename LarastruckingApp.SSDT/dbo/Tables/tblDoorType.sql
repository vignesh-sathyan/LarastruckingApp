CREATE TABLE [dbo].[tblDoorType] (
    [DoorTypeId] INT          IDENTITY (1, 1) NOT NULL,
    [DoorType]   VARCHAR (50) NULL,
    CONSTRAINT [PK_tblDoorType] PRIMARY KEY CLUSTERED ([DoorTypeId] ASC)
);

