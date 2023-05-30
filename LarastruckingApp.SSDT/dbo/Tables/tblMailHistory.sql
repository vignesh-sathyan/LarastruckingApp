CREATE TABLE [dbo].[tblMailHistory] (
    [MailHistoryId]  INT             IDENTITY (1, 1) NOT NULL,
    [MailPurpose]    VARCHAR (100)   NULL,
    [Status]         VARCHAR (50)    NULL,
    [ToMail]         VARCHAR (MAX)   NULL,
    [ToMailCC]       VARCHAR (MAX)   NULL,
    [ToMailBCC]      VARCHAR (MAX)   NULL,
    [MailSubject]    VARCHAR (MAX)   NULL,
    [MailBody]       VARCHAR (MAX)   NULL,
    [FileByte]       VARBINARY (MAX) NULL,
    [ErrorMessage]   NVARCHAR (MAX)  NULL,
    [Trace]          NVARCHAR (MAX)  NULL,
    [InnerException] NVARCHAR (MAX)  NULL,
    [CreatedBy]      INT             NOT NULL,
    [CreatedOn]      DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([MailHistoryId] ASC)
);

