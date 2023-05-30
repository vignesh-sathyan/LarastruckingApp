CREATE TABLE [dbo].[tblDelivery] (
    [DeliveryID]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [DriverID]           BIGINT        NULL,
    [AWB]                VARCHAR (MAX) NULL,
    [TransportVehicalID] BIGINT        NULL,
    [DeliveryRefNo]      VARCHAR (MAX) NULL,
    [CreatedBy]          BIGINT        NULL,
    [CreatedOn]          DATETIME      NULL,
    [ModifiedBy]         BIGINT        NULL,
    [ModifiedDate]       DATETIME      NULL,
    [IsDeleted]          BIT           NULL,
    PRIMARY KEY CLUSTERED ([DeliveryID] ASC),
    CONSTRAINT [FK_tblDelivery_tblTransportVehical] FOREIGN KEY ([TransportVehicalID]) REFERENCES [dbo].[tblTransportVehical] ([TransportVehicalID])
);

