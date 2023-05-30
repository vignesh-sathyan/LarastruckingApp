CREATE TABLE [dbo].[tblShipmentRefNo] (
    [ShipmentRefNoId] INT          IDENTITY (1, 1) NOT NULL,
    [ShipmenReftNo]   VARCHAR (50) NULL,
    [CreatedOn]       DATETIME     NOT NULL,
    PRIMARY KEY CLUSTERED ([ShipmentRefNoId] ASC)
);

