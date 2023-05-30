CREATE TABLE [dbo].[tblFumigationStatusHistory] (
    [FumigationStatusHistoryId] INT           IDENTITY (1, 1) NOT NULL,
    [FumigationId]              INT           NULL,
    [StatusId]                  INT           NOT NULL,
    [SubStatusId]               INT           NULL,
    [Reason]                    VARCHAR (MAX) NULL,
    [CreatedBy]                 INT           NOT NULL,
    [CreatedOn]                 DATETIME      CONSTRAINT [DF_tblFumigationStatusHistory_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_tblFumigationStatusHistory] PRIMARY KEY CLUSTERED ([FumigationStatusHistoryId] ASC),
    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[tblUser] ([Userid]),
    FOREIGN KEY ([FumigationId]) REFERENCES [dbo].[tblFumigation] ([FumigationId]),
    FOREIGN KEY ([StatusId]) REFERENCES [dbo].[tblShipmentStatus] ([StatusId]),
    FOREIGN KEY ([StatusId]) REFERENCES [dbo].[tblShipmentStatus] ([StatusId]),
    FOREIGN KEY ([StatusId]) REFERENCES [dbo].[tblShipmentStatus] ([StatusId]),
    FOREIGN KEY ([SubStatusId]) REFERENCES [dbo].[tblShipmentSubStatus] ([SubStatusId])
);



