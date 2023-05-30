CREATE TYPE [dbo].[UT_QuoteAccessorialPrice] AS TABLE (
    [RouteNo]              INT             NULL,
    [AccessorialFeeTypeId] INT             NULL,
    [Unit]                 DECIMAL (18, 2) NULL,
    [AmtPerUnit]           DECIMAL (18, 2) NULL,
    [Amount]               DECIMAL (18, 2) NULL);

