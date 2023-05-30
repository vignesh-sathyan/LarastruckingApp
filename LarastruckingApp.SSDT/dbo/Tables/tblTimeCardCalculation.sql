CREATE TABLE [dbo].[tblTimeCardCalculation] (
    [ID]           INT             IDENTITY (1, 1) NOT NULL,
    [UserId]       INT             NULL,
    [WeekStartDay] DATETIME        NOT NULL,
    [WeekEndDay]   DATETIME        NOT NULL,
    [HourlyRate]   DECIMAL (18, 2) NULL,
    [TotalPay]     DECIMAL (18, 2) NULL,
    [Loan]         DECIMAL (18, 2) NULL,
    [Deduction]    DECIMAL (18, 2) NULL,
    [Description]  VARCHAR (200)   NULL,
    [CreatedOn]    DATETIME        NULL,
    [CreatedBy]    INT             NULL,
    [Remaining]    DECIMAL (18, 2) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

