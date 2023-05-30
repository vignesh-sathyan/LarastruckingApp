CREATE TABLE [dbo].[tblShipmentFreightDetail] (
    [ShipmentBaseFreightDetailId] INT             IDENTITY (1, 1) NOT NULL,
    [ShipmentId]                  INT             NULL,
    [ShipmentRouteStopeId]        INT             NULL,
    [Commodity]                   VARCHAR (50)    NULL,
    [FreightTypeId]               INT             NULL,
    [PricingMethodId]             INT             NULL,
    [MinFee]                      DECIMAL (18, 2) NULL,
    [UpTo]                        DECIMAL (18, 2) NULL,
    [UnitPrice]                   DECIMAL (18, 2) NULL,
    [Hazardous]                   BIT             CONSTRAINT [DF__tblShipme__Hazar__5CC1BC92] DEFAULT ((0)) NOT NULL,
    [Temperature]                 DECIMAL (10, 2) NULL,
    [TemperatureType]             VARCHAR (5)     NULL,
    [QuantityNweight]             DECIMAL (18, 2) NULL,
    [PcsType]                     INT             NULL,
    [NoOfBox]                     INT             NULL,
    [Weight]                      DECIMAL (18, 2) NULL,
    [Unit]                        VARCHAR (50)    NULL,
    [TotalPrice]                  DECIMAL (18, 2) NULL,
    [IsDeleted]                   BIT             CONSTRAINT [DF_tblShipmentFreightDetail_IsDeleted] DEFAULT ((0)) NOT NULL,
    [DeletedOn]                   DATETIME        NULL,
    [DeletedBy]                   INT             NULL,
    [Comments]                    VARCHAR (200)   NULL,
    [TrailerCount]                INT             NULL,
    [PartialBox]                  INT             NULL,
    [PartialPallete]              INT             NULL,
    [IsPartialShipment]           BIT             CONSTRAINT [DF_tblShipmentFreightDetail_IsPartialShipment] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__tblShipm__285EA10540040082] PRIMARY KEY CLUSTERED ([ShipmentBaseFreightDetailId] ASC),
    CONSTRAINT [FK__tblShipme__Freig__795DFB40] FOREIGN KEY ([FreightTypeId]) REFERENCES [dbo].[tblFreightType] ([FreightTypeId]),
    CONSTRAINT [FK__tblShipme__Prici__7A521F79] FOREIGN KEY ([PricingMethodId]) REFERENCES [dbo].[tblPricingMethod] ([PricingMethodId]),
    CONSTRAINT [FK__tblShipme__Shipm__42ACE4D4] FOREIGN KEY ([ShipmentId]) REFERENCES [dbo].[tblShipment] ([ShipmentId]),
    CONSTRAINT [FK__tblShipme__Shipm__7B4643B2] FOREIGN KEY ([ShipmentRouteStopeId]) REFERENCES [dbo].[tblShipmentRoutesStop] ([ShippingRoutesId])
);












GO
CREATE NONCLUSTERED INDEX [IX_tblShipmentFreightDetail]
    ON [dbo].[tblShipmentFreightDetail]([ShipmentId] ASC, [ShipmentRouteStopeId] ASC, [QuantityNweight] ASC, [NoOfBox] ASC, [Weight] ASC, [Unit] ASC, [IsDeleted] ASC);

