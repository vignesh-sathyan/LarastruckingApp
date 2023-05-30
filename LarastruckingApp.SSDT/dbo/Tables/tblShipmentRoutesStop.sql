CREATE TABLE [dbo].[tblShipmentRoutesStop] (
    [ShippingRoutesId]              INT            IDENTITY (1, 1) NOT NULL,
    [ShippingId]                    INT            NULL,
    [RouteNo]                       INT            NULL,
    [PickupLocationId]              INT            NULL,
    [DeliveryLocationId]            INT            NULL,
    [PickDateTime]                  DATETIME       NULL,
    [DeliveryDateTime]              DATETIME       NULL,
    [PickUpDateTimeTo]              DATETIME       NULL,
    [DeliveryDateTimeTo]            DATETIME       NULL,
    [Comment]                       VARCHAR (500)  NULL,
    [DriverPickupArrival]           DATETIME       NULL,
    [DriverPickupDeparture]         DATETIME       NULL,
    [DriverDeliveryArrival]         DATETIME       NULL,
    [DriverDeliveryDeparture]       DATETIME       NULL,
    [IsDeleted]                     BIT            CONSTRAINT [DF_tblShipmentRoutesStop_IsDeleted] DEFAULT ((0)) NOT NULL,
    [DeletedOn]                     DATETIME       CONSTRAINT [DF_tblShipmentRoutesStop_] DEFAULT ((0)) NULL,
    [DeletedBy]                     INT            NULL,
    [DigitalSignature]              NVARCHAR (MAX) NULL,
    [ReceiverName]                  NVARCHAR (100) NULL,
    [IsAppointmentRequired]         BIT            CONSTRAINT [DF_tblShipmentRoutesStop_IsAppointmentRequired] DEFAULT ((0)) NOT NULL,
    [IsPickUpWaitingTimeRequired]   BIT            CONSTRAINT [DF_tblShipmentRoutesStop_IsPickUpWaitingTimeRequireds] DEFAULT ((0)) NOT NULL,
    [IsDeliveryWaitingTimeRequired] BIT            CONSTRAINT [DF_tblShipmentRoutesStop_IsDeliveryWaitingTimeRequired] DEFAULT ((0)) NOT NULL,
    [DigitalSignaturePath]          VARCHAR (MAX)  NULL,
    CONSTRAINT [PK_tblShipmentRoutesStop] PRIMARY KEY CLUSTERED ([ShippingRoutesId] ASC),
    CONSTRAINT [FK__tblShipme__Shipp__7093AB15] FOREIGN KEY ([ShippingId]) REFERENCES [dbo].[tblShipment] ([ShipmentId]),
    CONSTRAINT [FK__tblShipme__Shipp__7187CF4E] FOREIGN KEY ([ShippingId]) REFERENCES [dbo].[tblShipment] ([ShipmentId])
);


























GO
CREATE NONCLUSTERED INDEX [IXShipmentRouteStop]
    ON [dbo].[tblShipmentRoutesStop]([ShippingId] DESC, [PickUpDateTimeTo] DESC);






GO
CREATE NONCLUSTERED INDEX [IXShipmentRouteStops]
    ON [dbo].[tblShipmentRoutesStop]([IsDeleted] ASC);

