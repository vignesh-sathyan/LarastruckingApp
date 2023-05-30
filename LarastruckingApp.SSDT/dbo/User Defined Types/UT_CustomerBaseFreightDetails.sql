CREATE TYPE [dbo].[UT_CustomerBaseFreightDetails] AS TABLE (
    [RouteNo]            INT             NULL,
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
    [QutWgtVlm]          DECIMAL (8, 2)  NULL,
    [TotalPrice]         DECIMAL (18, 2) NULL,
    [NoOfBox]            INT             NULL,
    [Weight]             DECIMAL (18, 2) NULL,
    [Unit]               NVARCHAR (50)   NULL,
    [TrailerCount]       INT             NULL);





