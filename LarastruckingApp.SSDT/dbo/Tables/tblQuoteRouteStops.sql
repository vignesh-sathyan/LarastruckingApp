CREATE TABLE [dbo].[tblQuoteRouteStops] (
    [QuoteRouteStopsId]  INT      IDENTITY (1, 1) NOT NULL,
    [QuoteId]            INT      NULL,
    [RouteNo]            INT      NULL,
    [PickupLocationId]   INT      NULL,
    [DeliveryLocationId] INT      NULL,
    [PickDateTime]       DATETIME NULL,
    [DeliveryDateTime]   DATETIME NULL,
    CONSTRAINT [PK__tblQuote__0776267480300881] PRIMARY KEY CLUSTERED ([QuoteRouteStopsId] ASC),
    CONSTRAINT [FK__tblQuoteR__Deliv__2CBDA3B5] FOREIGN KEY ([DeliveryLocationId]) REFERENCES [dbo].[tblAddress] ([AddressId]),
    CONSTRAINT [FK__tblQuoteR__Deliv__44CA3770] FOREIGN KEY ([DeliveryLocationId]) REFERENCES [dbo].[tblAddress] ([AddressId]),
    CONSTRAINT [FK__tblQuoteR__Picku__2DB1C7EE] FOREIGN KEY ([PickupLocationId]) REFERENCES [dbo].[tblAddress] ([AddressId]),
    CONSTRAINT [FK__tblQuoteR__Picku__45BE5BA9] FOREIGN KEY ([PickupLocationId]) REFERENCES [dbo].[tblAddress] ([AddressId]),
    CONSTRAINT [FK__tblQuoteR__Quote__2EA5EC27] FOREIGN KEY ([QuoteId]) REFERENCES [dbo].[tblQuotes] ([QuoteId]),
    CONSTRAINT [FK__tblQuoteR__Quote__46B27FE2] FOREIGN KEY ([QuoteId]) REFERENCES [dbo].[tblQuotes] ([QuoteId])
);



