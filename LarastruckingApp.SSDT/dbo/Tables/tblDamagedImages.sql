CREATE TABLE [dbo].[tblDamagedImages] (
    [DamagedID]        INT           IDENTITY (1, 1) NOT NULL,
    [ShipmentRouteID]  INT           NULL,
    [ImageName]        VARCHAR (MAX) NULL,
    [ImageDescription] VARCHAR (MAX) NULL,
    [ImageUrl]         VARCHAR (MAX) NULL,
    [CreatedBy]        INT           NOT NULL,
    [CreatedOn]        DATETIME      NOT NULL,
    [IsDeleted]        BIT           CONSTRAINT [DF_tblDamagedImages_IsDeleted] DEFAULT ((0)) NOT NULL,
    [DeletedOn]        DATETIME      NULL,
    [DeletedBy]        INT           NULL,
    [IsApproved]       BIT           CONSTRAINT [DF_tblDamagedImages_IsApproved] DEFAULT ((0)) NOT NULL,
    [ApprovedBy]       INT           NULL,
    [ApprovedOn]       DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([DamagedID] ASC),
    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[tblUser] ([Userid]),
    CONSTRAINT [FK__tblDamage__Shipm__3572E547] FOREIGN KEY ([ShipmentRouteID]) REFERENCES [dbo].[tblShipmentRoutesStop] ([ShippingRoutesId])
);









