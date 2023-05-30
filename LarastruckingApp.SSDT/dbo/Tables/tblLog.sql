CREATE TABLE [dbo].[tblLog] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [ErrorMessage]   NVARCHAR (500) NULL,
    [Trace]          NVARCHAR (MAX) NULL,
    [InnerException] NVARCHAR (MAX) NULL,
    [CreatedDate]    DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

