CREATE TABLE [dbo].[tblShipment] (
    [ShipmentId]        INT             IDENTITY (1, 1) NOT NULL,
    [QuoteId]           INT             NULL,
    [StatusId]          INT             NULL,
    [SubStatusId]       INT             NULL,
    [CustomerId]        BIGINT          NULL,
    [RequestedBy]       VARCHAR (100)   NULL,
    [Reason]            VARCHAR (1000)  NULL,
    [ShipmentRefNo]     VARCHAR (50)    NULL,
    [AirWayBill]        VARCHAR (50)    NULL,
    [CustomerPO]        VARCHAR (50)    NULL,
    [OrderNo]           VARCHAR (50)    NULL,
    [CustomerRef]       VARCHAR (50)    NULL,
    [ContainerNo]       VARCHAR (50)    NULL,
    [PurchaseDoc]       VARCHAR (50)    NULL,
    [EquipmentId]       INT             NOT NULL,
    [DriverId]          INT             NOT NULL,
    [FinalTotalAmount]  DECIMAL (18, 2) NULL,
    [DriverInstruction] VARCHAR (1000)  NULL,
    [IsDeleted]         BIT             CONSTRAINT [DF__tblShipme__IsDel__3C1FE2D6] DEFAULT ((0)) NOT NULL,
    [CreatedDate]       DATETIME        CONSTRAINT [DF__tblShipme__Creat__3D14070F] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]         INT             NOT NULL,
    [ModifiedDate]      DATETIME        NULL,
    [ModifiedBy]        INT             NULL,
    [DeletedOn]         DATETIME        CONSTRAINT [DF_tblShipment_DeletedOn] DEFAULT (NULL) NULL,
    [DeletedBy]         INT             CONSTRAINT [DF_tblShipment_DeletedBy] DEFAULT (NULL) NULL,
    [VendorNconsignee]  NVARCHAR (100)  NULL,
    [UploadedFileName]  VARCHAR (200)   NULL,
    [IsWaiting]         BIT             NULL,
    [IsReady]           BIT             CONSTRAINT [DF_tblShipment_IsReady] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__tblShipm__5CAD37EDC580917C] PRIMARY KEY CLUSTERED ([ShipmentId] ASC)
);




























GO
CREATE NONCLUSTERED INDEX [ix_tblShipment]
    ON [dbo].[tblShipment]([CustomerId] ASC, [AirWayBill] ASC, [CustomerPO] ASC, [OrderNo] ASC);




GO
CREATE NONCLUSTERED INDEX [ix_tblShipments]
    ON [dbo].[tblShipment]([StatusId] ASC, [IsDeleted] ASC);

