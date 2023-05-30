CREATE TYPE [dbo].[UT_ShipmentRouteStop1] AS TABLE (
    [RouteNo]            INT           NULL,
    [PickupLocationId]   INT           NULL,
    [PickDateTime]       DATETIME      NULL,
    [PickDateTimeTo]     DATETIME      NULL,
    [DeliveryLocationId] INT           NULL,
    [DeliveryDateTime]   DATETIME      NULL,
    [DeliveryDateTimeTo] DATETIME      NULL,
    [Comment]            VARCHAR (500) NULL);

