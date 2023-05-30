CREATE TYPE [dbo].[UT_ShipmentFreightDetails] AS TABLE (
    [PickupLocationId]   INT             NULL,
    [DeliveryLocationId] INT             NULL,
    [Commodity]          NVARCHAR (100)  NULL,
    [FreightTypeId]      INT             NULL,
    [PricingMethodId]    INT             NULL,
    [MinFee]             DECIMAL (18, 2) NULL,
    [Upto]               DECIMAL (18, 2) NULL,
    [UnitPrice]          DECIMAL (18, 2) NULL,
    [Hazardous]          BIT             DEFAULT ((0)) NOT NULL,
    [Temperature]        DECIMAL (8, 2)  NULL,
    [TemperatureType]    VARCHAR (50)    NULL,
    [QutWgtVlm]          DECIMAL (8, 2)  NULL,
    [TotalPrice]         DECIMAL (18, 2) NULL,
    [NoOfBox]            INT             NULL,
    [Weight]             DECIMAL (18, 2) NULL,
    [Unit]               VARCHAR (50)    NULL,
    [TrailerCount]       INT             NULL,
    [Comments]           VARCHAR (200)   NULL,
    [IsPartialShipment]  BIT             DEFAULT ((0)) NOT NULL,
    [PartialPallet]      INT             NULL,
    [PartialBox]         INT             NULL);









