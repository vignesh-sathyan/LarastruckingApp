CREATE TYPE [dbo].[UT_ShipmentRouteStop] AS TABLE (
    [RouteNo]                       INT           NULL,
    [PickupLocationId]              INT           NULL,
    [PickDateTime]                  DATETIME      NULL,
    [PickDateTimeTo]                DATETIME      NULL,
    [DeliveryLocationId]            INT           NULL,
    [DeliveryDateTime]              DATETIME      NULL,
    [DeliveryDateTimeTo]            DATETIME      NULL,
    [Comment]                       VARCHAR (500) NULL,
    [IsAppointmentRequired]         BIT           NOT NULL,
    [IsPickUpWaitingTimeRequired]   BIT           NOT NULL,
    [IsDeliveryWaitingTimeRequired] BIT           NOT NULL);



