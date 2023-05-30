CREATE TABLE [dbo].[tblQuoteAccessorialPrice] (
    [QuoteAccessorialPriceId] INT             IDENTITY (1, 1) NOT NULL,
    [QuoteId]                 INT             NULL,
    [QuoteRouteStopsId]       INT             NULL,
    [AccessorialFeeTypeId]    INT             NULL,
    [Unit]                    DECIMAL (18, 2) NULL,
    [AmtPerUnit]              DECIMAL (18, 2) NULL,
    [Amount]                  DECIMAL (18, 2) NULL,
    [IsDeleted]               BIT             CONSTRAINT [DF__tblQuoteA__IsDel__0A537D18] DEFAULT ((0)) NOT NULL,
    [DeletedOn]               DATETIME        NULL,
    [DeletedBy]               INT             NULL,
    [Reason]                  VARCHAR (200)   NULL,
    CONSTRAINT [PK__tblQuote__D03599D585B65BD0] PRIMARY KEY CLUSTERED ([QuoteAccessorialPriceId] ASC),
    CONSTRAINT [FK__tblQuoteA__Acces__095F58DF] FOREIGN KEY ([AccessorialFeeTypeId]) REFERENCES [dbo].[tblAccessorialFeesTypes] ([Id]),
    CONSTRAINT [FK__tblQuoteA__Quote__0777106D] FOREIGN KEY ([QuoteId]) REFERENCES [dbo].[tblQuotes] ([QuoteId]),
    CONSTRAINT [FK__tblQuoteA__Quote__086B34A6] FOREIGN KEY ([QuoteRouteStopsId]) REFERENCES [dbo].[tblQuoteRouteStops] ([QuoteRouteStopsId])
);



