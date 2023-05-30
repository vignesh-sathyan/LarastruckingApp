CREATE TABLE [dbo].[tblShipmentSubStatus] (
    [SubStatusId]          INT           IDENTITY (1, 1) NOT NULL,
    [StatusId]             INT           NOT NULL,
    [SubStatusName]        VARCHAR (100) NULL,
    [DisplayOrder]         INT           NOT NULL,
    [IsActive]             BIT           DEFAULT ((1)) NOT NULL,
    [IsDeleted]            BIT           DEFAULT ((0)) NOT NULL,
    [SpanishSubStatusName] VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([SubStatusId] ASC),
    FOREIGN KEY ([StatusId]) REFERENCES [dbo].[tblShipmentStatus] ([StatusId])
);



