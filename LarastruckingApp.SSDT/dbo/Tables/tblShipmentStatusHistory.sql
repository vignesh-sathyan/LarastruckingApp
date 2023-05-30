CREATE TABLE [dbo].[tblShipmentStatusHistory] (
    [ShipmentStatusHistoryId] INT           IDENTITY (1, 1) NOT NULL,
    [ShipmentId]              INT           NULL,
    [StatusId]                INT           NOT NULL,
    [SubStatusId]             INT           NULL,
    [Reason]                  VARCHAR (MAX) NULL,
    [CreatedBy]               INT           NOT NULL,
    [CreatedOn]               DATETIME      DEFAULT (getutcdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([ShipmentStatusHistoryId] ASC),
    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[tblUser] ([Userid]),
    FOREIGN KEY ([StatusId]) REFERENCES [dbo].[tblShipmentStatus] ([StatusId]),
    FOREIGN KEY ([StatusId]) REFERENCES [dbo].[tblShipmentStatus] ([StatusId]),
    FOREIGN KEY ([SubStatusId]) REFERENCES [dbo].[tblShipmentSubStatus] ([SubStatusId]),
    CONSTRAINT [FK__tblShipme__Shipm__18D6A699] FOREIGN KEY ([ShipmentId]) REFERENCES [dbo].[tblShipment] ([ShipmentId])
);








GO
CREATE NONCLUSTERED INDEX [ix_tblShipmentStatusHistory]
    ON [dbo].[tblShipmentStatusHistory]([ShipmentId] ASC);

