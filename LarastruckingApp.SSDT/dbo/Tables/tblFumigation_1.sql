﻿CREATE TABLE [dbo].[tblFumigation] (
    [FumigationId]     INT           IDENTITY (1, 1) NOT NULL,
    [CustomerId]       BIGINT        NULL,
    [StatusId]         INT           NULL,
    [SubStatusId]      INT           NULL,
    [Reason]           VARCHAR (MAX) NULL,
    [ShipmentRefNo]    VARCHAR (50)  NULL,
    [VendorNconsignee] VARCHAR (100) NULL,
    [RequestedBy]      VARCHAR (100) NULL,
    [Comments]         VARCHAR (MAX) NULL,
    [IsActive]         BIT           DEFAULT ((1)) NOT NULL,
    [CreatedOn]        DATETIME      NOT NULL,
    [CreatedBy]        INT           NOT NULL,
    [ModifiedOn]       DATETIME      NULL,
    [ModifiedBy]       INT           NULL,
    [IsDeleted]        BIT           DEFAULT ((0)) NOT NULL,
    [DeletedOn]        DATETIME      NULL,
    [DeletedBy]        INT           NULL,
    PRIMARY KEY CLUSTERED ([FumigationId] ASC),
    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[tblUser] ([Userid]),
    FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[tblCustomerRegistration] ([CustomerID]),
    FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[tblUser] ([Userid]),
    FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[tblUser] ([Userid]),
    FOREIGN KEY ([StatusId]) REFERENCES [dbo].[tblShipmentStatus] ([StatusId]),
    FOREIGN KEY ([SubStatusId]) REFERENCES [dbo].[tblShipmentSubStatus] ([SubStatusId])
);




GO
CREATE NONCLUSTERED INDEX [ix_tblFumigation]
    ON [dbo].[tblFumigation]([StatusId] ASC, [IsDeleted] ASC);

