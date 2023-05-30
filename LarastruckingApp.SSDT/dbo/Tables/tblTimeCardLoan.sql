CREATE TABLE [dbo].[tblTimeCardLoan] (
    [Id]        INT             IDENTITY (1, 1) NOT NULL,
    [UserId]    INT             NOT NULL,
    [Loan]      DECIMAL (18, 2) NULL,
    [IsPaid]    BIT             NULL,
    [CreatedOn] DATETIME        NOT NULL,
    [CreatedBy] INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

