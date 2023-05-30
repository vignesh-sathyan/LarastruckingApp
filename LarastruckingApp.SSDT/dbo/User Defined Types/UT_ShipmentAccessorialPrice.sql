CREATE TYPE [dbo].[UT_ShipmentAccessorialPrice] AS TABLE (
    [RouteNo]              INT             NULL,
    [AccessorialFeeTypeId] INT             NULL,
    [Unit]                 DECIMAL (18, 2) NULL,
    [AmtPerUnit]           DECIMAL (18, 2) NULL,
    [Amount]               DECIMAL (18, 2) NULL,
    [Reason]               VARCHAR (200)   NULL);



