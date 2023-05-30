CREATE TABLE [dbo].[tblTermAndCondition] (
    [TermAndConditionId] INT           IDENTITY (1, 1) NOT NULL,
    [TermAndCondition]   VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([TermAndConditionId] ASC)
);

