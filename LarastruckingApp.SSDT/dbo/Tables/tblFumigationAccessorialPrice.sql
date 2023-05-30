CREATE TABLE [dbo].[tblFumigationAccessorialPrice] (
    [FumigationAccessorialPriceId] INT             IDENTITY (1, 1) NOT NULL,
    [FumigationId]                 INT             NULL,
    [FumigationRoutesId]           INT             NULL,
    [AccessorialFeeTypeId]         INT             NULL,
    [Unit]                         DECIMAL (18, 2) NULL,
    [AmtPerUnit]                   DECIMAL (18, 2) NULL,
    [Amount]                       DECIMAL (18, 2) NULL,
    [IsDeleted]                    BIT             DEFAULT ((0)) NOT NULL,
    [DeletedOn]                    DATETIME        NULL,
    [DeletedBy]                    INT             NULL,
    [Reason]                       VARCHAR (200)   NULL,
    PRIMARY KEY CLUSTERED ([FumigationAccessorialPriceId] ASC),
    FOREIGN KEY ([AccessorialFeeTypeId]) REFERENCES [dbo].[tblAccessorialFeesTypes] ([Id]),
    FOREIGN KEY ([FumigationId]) REFERENCES [dbo].[tblFumigation] ([FumigationId]),
    CONSTRAINT [FK__tblFumiga__Fumig__453F38BC] FOREIGN KEY ([FumigationRoutesId]) REFERENCES [dbo].[tblFumigationRouts] ([FumigationRoutsId])
);





