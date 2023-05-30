CREATE TABLE [dbo].[tblShipmentAccessorialPrice] (
    [ShipmentAccessorialPriceId] INT             IDENTITY (1, 1) NOT NULL,
    [ShipmentId]                 INT             NULL,
    [ShipmentRouteStopeId]       INT             NULL,
    [AccessorialFeeTypeId]       INT             NULL,
    [Unit]                       DECIMAL (18, 2) NULL,
    [AmtPerUnit]                 DECIMAL (18, 2) NULL,
    [Amount]                     DECIMAL (18, 2) NULL,
    [IsDeleted]                  BIT             CONSTRAINT [DF_tblShipmentAccessorialPrice_IsDeleted] DEFAULT ((0)) NOT NULL,
    [DeletedOn]                  DATETIME        NULL,
    [DeletedBy]                  INT             NULL,
    [Reason]                     VARCHAR (200)   NULL,
    PRIMARY KEY CLUSTERED ([ShipmentAccessorialPriceId] ASC),
    FOREIGN KEY ([AccessorialFeeTypeId]) REFERENCES [dbo].[tblAccessorialFeesTypes] ([Id]),
    CONSTRAINT [FK__tblShipme__Shipm__3CF40B7E] FOREIGN KEY ([ShipmentId]) REFERENCES [dbo].[tblShipment] ([ShipmentId]),
    CONSTRAINT [FK__tblShipme__Shipm__7775B2CE] FOREIGN KEY ([ShipmentRouteStopeId]) REFERENCES [dbo].[tblShipmentRoutesStop] ([ShippingRoutesId])
);









