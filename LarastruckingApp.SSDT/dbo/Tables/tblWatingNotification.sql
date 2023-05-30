CREATE TABLE [dbo].[tblWatingNotification] (
    [WatingNotificationId]     INT           IDENTITY (1, 1) NOT NULL,
    [ShipmentId]               INT           NULL,
    [ShipmentRouteId]          INT           NULL,
    [PickupArrivedOn]          DATETIME      NULL,
    [PickupDepartedOn]         DATETIME      NULL,
    [DeliveryArrivedOn]        DATETIME      NULL,
    [DeliveryDepartedOn]       DATETIME      NULL,
    [PickUpLocationId]         INT           NULL,
    [CustomerId]               BIGINT        NULL,
    [EquipmentNo]              VARCHAR (200) NULL,
    [DriverId]                 INT           NULL,
    [IsDelivered]              BIT           DEFAULT ((0)) NOT NULL,
    [IsEmailSentPWS]           BIT           DEFAULT ((0)) NOT NULL,
    [IsEmailSentPWE]           BIT           DEFAULT ((0)) NOT NULL,
    [IsEmailSentDWS]           BIT           DEFAULT ((0)) NOT NULL,
    [IsEmailSentDWE]           BIT           DEFAULT ((0)) NOT NULL,
    [DeliveryLocationId]       INT           NULL,
    [TotalPickupWaitingTime]   VARCHAR (10)  NULL,
    [TotalDeliveryWaitingTime] VARCHAR (10)  NULL,
    CONSTRAINT [PK_tblWatingNotification] PRIMARY KEY CLUSTERED ([WatingNotificationId] ASC),
    FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[tblCustomerRegistration] ([CustomerID]),
    FOREIGN KEY ([DriverId]) REFERENCES [dbo].[tblDriver] ([DriverID]),
    FOREIGN KEY ([ShipmentId]) REFERENCES [dbo].[tblShipment] ([ShipmentId]),
    CONSTRAINT [FK__tblWating__Shipm__53584DE9] FOREIGN KEY ([ShipmentRouteId]) REFERENCES [dbo].[tblShipmentRoutesStop] ([ShippingRoutesId]),
    CONSTRAINT [FK__tblWating__Shipm__544C7222] FOREIGN KEY ([ShipmentRouteId]) REFERENCES [dbo].[tblShipmentRoutesStop] ([ShippingRoutesId])
);





