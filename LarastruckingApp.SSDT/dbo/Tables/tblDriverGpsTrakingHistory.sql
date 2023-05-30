CREATE TABLE [dbo].[tblDriverGpsTrakingHistory] (
    [DriverGpsID]       INT           IDENTITY (1, 1) NOT NULL,
    [UserId]            INT           NULL,
    [Latitude]          VARCHAR (MAX) NULL,
    [longitude]         VARCHAR (MAX) NULL,
    [CreatedOn]         DATETIME      NULL,
    [ShipmentId]        INT           NULL,
    [ShipmentRouteId]   INT           NULL,
    [Event]             VARCHAR (200) NULL,
    [FumigationId]      INT           NULL,
    [FumigationRoutsId] INT           NULL,
    CONSTRAINT [PK_tblDriverGpsTrakingHistory] PRIMARY KEY CLUSTERED ([DriverGpsID] ASC),
    FOREIGN KEY ([FumigationId]) REFERENCES [dbo].[tblFumigation] ([FumigationId]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[tblUser] ([Userid]),
    CONSTRAINT [FK__tblDriver__Fumig__7E77B618] FOREIGN KEY ([FumigationRoutsId]) REFERENCES [dbo].[tblFumigationRouts] ([FumigationRoutsId]),
    CONSTRAINT [FK__tblDriver__Shipm__51DA19CB] FOREIGN KEY ([ShipmentId]) REFERENCES [dbo].[tblShipment] ([ShipmentId]),
    CONSTRAINT [FK__tblDriver__Shipm__52CE3E04] FOREIGN KEY ([ShipmentRouteId]) REFERENCES [dbo].[tblShipmentRoutesStop] ([ShippingRoutesId])
);









