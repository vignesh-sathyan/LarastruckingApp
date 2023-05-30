CREATE TYPE [dbo].[UT_FumigationAccessorialPrice] AS TABLE (
    [FumigationAccessorialPriceId] INT             NULL,
    [RouteNo]                      INT             NULL,
    [AccessorialFeeTypeId]         INT             NULL,
    [Unit]                         DECIMAL (18, 2) NULL,
    [AmtPerUnit]                   DECIMAL (18, 2) NULL,
    [Amount]                       DECIMAL (18, 2) NULL,
    [IsDeleted]                    BIT             DEFAULT ((0)) NULL,
    [Reason]                       VARCHAR (200)   NULL);



