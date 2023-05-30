CREATE TABLE [dbo].[tblQuotes] (
    [QuoteId]          INT             IDENTITY (1, 1) NOT NULL,
    [CustomerId]       BIGINT          NULL,
    [QuotesName]       VARCHAR (200)   NULL,
    [QuoteDate]        DATETIME        NOT NULL,
    [ValidUptoDate]    DATETIME        NOT NULL,
    [FinalTotalAmount] DECIMAL (18, 2) NULL,
    [QuoteStatusId]    INT             NULL,
    [IsDeleted]        BIT             DEFAULT ((0)) NOT NULL,
    [CreatedDate]      DATETIME        DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]        INT             NOT NULL,
    [ModifiedDate]     DATETIME        NULL,
    [ModifiedBy]       INT             NULL,
    PRIMARY KEY CLUSTERED ([QuoteId] ASC),
    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[tblUser] ([Userid]),
    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[tblUser] ([Userid]),
    CONSTRAINT [FK__tblQuotes__Custo__308E3499] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[tblCustomerRegistration] ([CustomerID]),
    CONSTRAINT [FK__tblQuotes__Custo__489AC854] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[tblCustomerRegistration] ([CustomerID])
);



