CREATE TABLE [dbo].[tblAssessorialPrice] (
    [AssessorialPriceId]    INT             IDENTITY (1, 1) NOT NULL,
    [QuoteId]               INT             NULL,
    [QuoteRouteStopsId]     INT             NULL,
    [RouteNo]               INT             NULL,
    [LoadingPerUnit]        DECIMAL (18, 2) NULL,
    [LoadingAmount]         DECIMAL (18, 2) NULL,
    [UnloadingPerUnit]      DECIMAL (18, 2) NULL,
    [UnloadingAmount]       DECIMAL (18, 2) NULL,
    [PalletExchangePerUnit] DECIMAL (18, 2) NULL,
    [PalletExchangeAmount]  DECIMAL (18, 2) NULL,
    [DieselRefuelingAmount] DECIMAL (18, 2) NULL,
    [OvernightAmount]       DECIMAL (18, 2) NULL,
    [SameDayAmount]         DECIMAL (18, 2) NULL,
    [TotalAssessorialFee]   DECIMAL (18, 2) NULL,
    CONSTRAINT [PK__tblAsses__5F6578D11FBE649C] PRIMARY KEY CLUSTERED ([AssessorialPriceId] ASC),
    CONSTRAINT [FK__tblAssess__Quote__19AACF41] FOREIGN KEY ([QuoteId]) REFERENCES [dbo].[tblQuotes] ([QuoteId]),
    CONSTRAINT [FK__tblAssess__Quote__1A9EF37A] FOREIGN KEY ([QuoteRouteStopsId]) REFERENCES [dbo].[tblQuoteRouteStops] ([QuoteRouteStopsId]),
    CONSTRAINT [FK__tblAssess__Quote__2CF2ADDF] FOREIGN KEY ([QuoteId]) REFERENCES [dbo].[tblQuotes] ([QuoteId]),
    CONSTRAINT [FK__tblAssess__Quote__2DE6D218] FOREIGN KEY ([QuoteRouteStopsId]) REFERENCES [dbo].[tblQuoteRouteStops] ([QuoteRouteStopsId])
);



