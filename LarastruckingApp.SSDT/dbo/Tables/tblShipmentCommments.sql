CREATE TABLE [dbo].[tblShipmentCommments] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [ShipmentId] INT           NULL,
    [Comment]    VARCHAR (200) NULL,
    [CommentBy]  VARCHAR (10)  NULL,
    [CreatedBy]  INT           NULL,
    [CreatedOn]  DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ShipmentId]) REFERENCES [dbo].[tblShipment] ([ShipmentId])
);

