CREATE TABLE [dbo].[tblFumigationRouts] (
    [FumigationRoutsId]       INT             IDENTITY (1, 1) NOT NULL,
    [RouteNo]                 INT             NULL,
    [FumigationId]            INT             NULL,
    [FumigationTypeId]        INT             NULL,
    [AirWayBill]              VARCHAR (50)    NULL,
    [CustomerPO]              VARCHAR (50)    NULL,
    [ContainerNo]             VARCHAR (50)    NULL,
    [PickUpLocation]          INT             NULL,
    [PickUpArrival]           DATETIME        NULL,
    [FumigationSite]          INT             NULL,
    [FumigationArrival]       DATETIME        NULL,
    [ReleaseDate]             DATETIME        NULL,
    [DepartureDate]           DATETIME        NULL,
    [DeliveryLocation]        INT             NULL,
    [DeliveryArrival]         DATETIME        NULL,
    [Commodity]               VARCHAR (50)    NULL,
    [PalletCount]             DECIMAL (18, 2) NULL,
    [BoxCount]                DECIMAL (18, 2) NULL,
    [BoxType]                 INT             NULL,
    [Temperature]             DECIMAL (10, 2) NULL,
    [TemperatureType]         VARCHAR (5)     NULL,
    [TrailerDays]             DECIMAL (18, 2) NULL,
    [PricingMethod]           INT             NULL,
    [MinFee]                  DECIMAL (18, 2) NULL,
    [AddFee]                  DECIMAL (18, 2) NULL,
    [TrailerPosition]         VARCHAR (50)    NULL,
    [TotalFee]                DECIMAL (18, 2) NULL,
    [IsDeleted]               BIT             CONSTRAINT [DF__tblFumiga__IsDel__168449D3] DEFAULT ((0)) NOT NULL,
    [DeletedOn]               DATETIME        CONSTRAINT [DF_tblFumigationRouts_DeletedOn] DEFAULT ((0)) NULL,
    [DeletedBy]               INT             NULL,
    [DriverPickupArrival]     DATETIME        NULL,
    [DriverPickupDeparture]   DATETIME        NULL,
    [DriverDeliveryArrival]   DATETIME        NULL,
    [DriverDeliveryDeparture] DATETIME        NULL,
    [DigitalSignature]        NVARCHAR (MAX)  NULL,
    [ReceiverName]            NVARCHAR (100)  NULL,
    [UpTo]                    DECIMAL (18, 2) NULL,
    [DriverFumigationIn]      DATETIME        NULL,
    [DriverLoadingStartTime]  DATETIME        NULL,
    [DriverLoadingFinishTime] DATETIME        NULL,
    [DriverFumigationRelease] DATETIME        NULL,
    [DigitalSignaturePath]    VARCHAR (MAX)   NULL,
    [VendorNConsignee]        VARCHAR (200)   NULL,
    CONSTRAINT [PK__tblFumig__83323E0E9CD70F15] PRIMARY KEY CLUSTERED ([FumigationRoutsId] ASC)
);













