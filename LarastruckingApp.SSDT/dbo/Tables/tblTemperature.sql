CREATE TABLE [dbo].[tblTemperature] (
    [TemperatureID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ShipmentID]    BIGINT        NULL,
    [DeliveryID]    BIGINT        NULL,
    [Temperature]   VARCHAR (100) NULL,
    [CreatedBy]     BIGINT        NULL,
    [CreatedOn]     DATETIME      NULL,
    [ModifiedBy]    BIGINT        NULL,
    [ModifiedDate]  DATETIME      NULL,
    [PackageID]     BIGINT        NULL,
    PRIMARY KEY CLUSTERED ([TemperatureID] ASC),
    FOREIGN KEY ([DeliveryID]) REFERENCES [dbo].[tblDelivery] ([DeliveryID]),
    FOREIGN KEY ([DeliveryID]) REFERENCES [dbo].[tblDelivery] ([DeliveryID]),
    FOREIGN KEY ([PackageID]) REFERENCES [dbo].[tblPackage] ([PackageID]),
    FOREIGN KEY ([PackageID]) REFERENCES [dbo].[tblPackage] ([PackageID])
);





