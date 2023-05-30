CREATE TYPE [dbo].[UT_QuoteRouteStops] AS TABLE (
    [RouteNo]            INT      NULL,
    [PickupLocationId]   INT      NULL,
    [DeliveryLocationId] INT      NULL,
    [PickDateTime]       DATETIME NULL,
    [DeliveryDateTime]   DATETIME NULL);



