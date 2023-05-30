CREATE TABLE [dbo].[tblCustomerBaseFreightDetails] (
    [CustomerBaseFreightDetailId] INT             IDENTITY (1, 1) NOT NULL,
    [QuoteId]                     INT             NULL,
    [RouteNo]                     INT             NULL,
    [QuoteRouteStopsId]           INT             NULL,
    [PickupLocationId]            INT             NULL,
    [DeliveryLocationId]          INT             NULL,
    [Commodity]                   NVARCHAR (100)  NULL,
    [FreightTypeId]               INT             NULL,
    [PricingMethodId]             INT             NULL,
    [MinFee]                      DECIMAL (18, 2) NULL,
    [Upto]                        DECIMAL (18, 2) NULL,
    [UnitPrice]                   DECIMAL (18, 2) NULL,
    [Hazardous]                   BIT             CONSTRAINT [DF__tblCustom__Hazar__114A936A] DEFAULT ((0)) NOT NULL,
    [Temperature]                 DECIMAL (18, 2) NULL,
    [QutWgtVlm]                   DECIMAL (18, 2) NULL,
    [TotalPrice]                  DECIMAL (18, 2) NULL,
    [NoOfBox]                     INT             NULL,
    [Weight]                      DECIMAL (18, 2) NULL,
    [Unit]                        VARCHAR (50)    NULL,
    [TrailerCount]                INT             NULL,
    CONSTRAINT [PK__tblCusto__D1F24343465323B1] PRIMARY KEY CLUSTERED ([CustomerBaseFreightDetailId] ASC),
    CONSTRAINT [FK__tblCustom__Deliv__22401542] FOREIGN KEY ([DeliveryLocationId]) REFERENCES [dbo].[tblAddress] ([AddressId]),
    CONSTRAINT [FK__tblCustom__Deliv__3587F3E0] FOREIGN KEY ([DeliveryLocationId]) REFERENCES [dbo].[tblAddress] ([AddressId]),
    CONSTRAINT [FK__tblCustom__Freig__2334397B] FOREIGN KEY ([FreightTypeId]) REFERENCES [dbo].[tblFreightType] ([FreightTypeId]),
    CONSTRAINT [FK__tblCustom__Freig__367C1819] FOREIGN KEY ([FreightTypeId]) REFERENCES [dbo].[tblFreightType] ([FreightTypeId]),
    CONSTRAINT [FK__tblCustom__Picku__24285DB4] FOREIGN KEY ([PickupLocationId]) REFERENCES [dbo].[tblAddress] ([AddressId]),
    CONSTRAINT [FK__tblCustom__Picku__37703C52] FOREIGN KEY ([PickupLocationId]) REFERENCES [dbo].[tblAddress] ([AddressId]),
    CONSTRAINT [FK__tblCustom__Prici__251C81ED] FOREIGN KEY ([PricingMethodId]) REFERENCES [dbo].[tblPricingMethod] ([PricingMethodId]),
    CONSTRAINT [FK__tblCustom__Prici__3864608B] FOREIGN KEY ([PricingMethodId]) REFERENCES [dbo].[tblPricingMethod] ([PricingMethodId]),
    CONSTRAINT [FK__tblCustom__Quote__2610A626] FOREIGN KEY ([QuoteId]) REFERENCES [dbo].[tblQuotes] ([QuoteId]),
    CONSTRAINT [FK__tblCustom__Quote__2704CA5F] FOREIGN KEY ([QuoteRouteStopsId]) REFERENCES [dbo].[tblQuoteRouteStops] ([QuoteRouteStopsId]),
    CONSTRAINT [FK__tblCustom__Quote__395884C4] FOREIGN KEY ([QuoteId]) REFERENCES [dbo].[tblQuotes] ([QuoteId]),
    CONSTRAINT [FK__tblCustom__Quote__3A4CA8FD] FOREIGN KEY ([QuoteRouteStopsId]) REFERENCES [dbo].[tblQuoteRouteStops] ([QuoteRouteStopsId])
);







