CREATE TYPE [dbo].[UT_FumigationRoutes] AS TABLE (
    [FumigationRoutsId] INT             NULL,
    [RouteNo]           INT             NULL,
    [FumigationId]      INT             NULL,
    [FumigationTypeId]  INT             NULL,
    [AirWayBill]        VARCHAR (50)    NULL,
    [CustomerPO]        VARCHAR (50)    NULL,
    [ContainerNo]       VARCHAR (50)    NULL,
    [PickUpLocation]    INT             NULL,
    [PickUpArrival]     DATETIME        NULL,
    [FumigationSite]    INT             NULL,
    [FumigationArrival] DATETIME        NULL,
    [DeliveryLocation]  INT             NULL,
    [DeliveryArrival]   DATETIME        NULL,
    [PalletCount]       DECIMAL (18, 2) NULL,
    [BoxCount]          DECIMAL (18, 2) NULL,
    [BoxType]           INT             NULL,
    [Temperature]       DECIMAL (18, 2) NULL,
    [TemperatureType]   VARCHAR (5)     NULL,
    [MinFee]            DECIMAL (18, 2) NULL,
    [AddFee]            DECIMAL (18, 2) NULL,
    [UpTo]              DECIMAL (18, 2) NULL,
    [TrailerPosition]   VARCHAR (50)    NULL,
    [TotalFee]          DECIMAL (18, 2) NULL,
    [IsDeleted]         BIT             DEFAULT ((0)) NULL,
    [ReleaseDate]       DATETIME        NULL,
    [DepartureDate]     DATETIME        NULL,
    [Commodity]         VARCHAR (50)    NULL,
    [PricingMethod]     INT             NULL,
    [TrailerDays]       DECIMAL (18, 2) NULL,
    [VendorNConsignee]  VARCHAR (100)   NULL);









