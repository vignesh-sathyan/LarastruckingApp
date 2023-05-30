CREATE TABLE [dbo].[tblEquipmentStatus] (
    [VehicleStatusID]   INT           IDENTITY (1, 1) NOT NULL,
    [VehicleStatusName] VARCHAR (300) NULL,
    [CreatedBy]         BIGINT        NULL,
    [CreatedOn]         DATETIME      NULL,
    [ModifiedBy]        BIGINT        NULL,
    [ModifiedOn]        DATETIME      NULL,
    [IsDeleted]         BIT           NULL,
    PRIMARY KEY CLUSTERED ([VehicleStatusID] ASC)
);

