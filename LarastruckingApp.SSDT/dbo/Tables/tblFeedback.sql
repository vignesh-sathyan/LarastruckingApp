CREATE TABLE [dbo].[tblFeedback] (
    [FeedbackId]    INT          IDENTITY (1, 1) NOT NULL,
    [FeedbackValue] VARCHAR (50) NULL,
    [Options]       VARCHAR (50) NULL,
    CONSTRAINT [PK_tblFeedback] PRIMARY KEY CLUSTERED ([FeedbackId] ASC)
);

