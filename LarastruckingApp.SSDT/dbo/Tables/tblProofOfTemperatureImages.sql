CREATE TABLE [dbo].[tblProofOfTemperatureImages] (
    [ImageId]                 INT           IDENTITY (1, 1) NOT NULL,
    [ImageName]               VARCHAR (MAX) NULL,
    [ImageDescription]        VARCHAR (MAX) NULL,
    [ImageUrl]                VARCHAR (MAX) NULL,
    [ShipmentRouteId]         INT           NULL,
    [ActualTemperature]       VARCHAR (50)  NULL,
    [CreatedBy]               INT           NOT NULL,
    [CreatedOn]               DATETIME      NOT NULL,
    [ShipmentFreightDetailId] INT           NULL,
    [IsDeleted]               BIT           CONSTRAINT [DF_tblProofOfTemperatureImages_IsDeleted] DEFAULT ((0)) NOT NULL,
    [DeletedOn]               DATETIME      NULL,
    [DeletedBy]               INT           NULL,
    [IsApproved]              BIT           CONSTRAINT [DF_tblProofOfTemperatureImages_IsApproved] DEFAULT ((0)) NOT NULL,
    [ApprovedBy]              INT           NULL,
    [ApprovedOn]              DATETIME      NULL,
    [IsLoading]               BIT           CONSTRAINT [DF_tblProofOfTemperatureImages_IsLoading] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_tblPreTripImages] PRIMARY KEY CLUSTERED ([ImageId] ASC),
    CONSTRAINT [FK__tblPreTri__Creat__2CDD9F46] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[tblUser] ([Userid]),
    CONSTRAINT [FK__tblProofO__Shipm__36670980] FOREIGN KEY ([ShipmentRouteId]) REFERENCES [dbo].[tblShipmentRoutesStop] ([ShippingRoutesId]),
    CONSTRAINT [FK__tblProofO__Shipm__42CCE065] FOREIGN KEY ([ShipmentFreightDetailId]) REFERENCES [dbo].[tblShipmentFreightDetail] ([ShipmentBaseFreightDetailId])
);











