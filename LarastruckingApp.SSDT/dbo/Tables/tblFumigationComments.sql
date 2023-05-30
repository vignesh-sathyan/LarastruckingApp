CREATE TABLE [dbo].[tblFumigationComments] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [FumigationId] INT           NULL,
    [Comment]      VARCHAR (200) NULL,
    [CommentBy]    VARCHAR (10)  NULL,
    [CreatedBy]    INT           NULL,
    [CreatedOn]    DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

