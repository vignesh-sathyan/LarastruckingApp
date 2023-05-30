CREATE TABLE [dbo].[tblEquipmentType] (
    [VehicleTypeID]   INT           IDENTITY (1, 1) NOT NULL,
    [VehicleTypeName] VARCHAR (300) NULL,
    [CreatedBy]       BIGINT        NULL,
    [CreatedOn]       DATETIME      NULL,
    [ModifiedBy]      BIGINT        NULL,
    [ModifiedOn]      DATETIME      NULL,
    [IsDeleted]       BIT           NULL,
    PRIMARY KEY CLUSTERED ([VehicleTypeID] ASC)
);

