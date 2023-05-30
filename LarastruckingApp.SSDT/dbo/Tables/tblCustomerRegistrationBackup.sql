CREATE TABLE [dbo].[tblCustomerRegistrationBackup] (
    [CustomerID]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [UserId]                INT           NULL,
    [CustomerName]          VARCHAR (100) NULL,
    [Contact]               VARCHAR (20)  NULL,
    [Website]               VARCHAR (50)  NULL,
    [Comments]              VARCHAR (MAX) NULL,
    [IsPickDropLocation]    BIT           NOT NULL,
    [IsDeleted]             BIT           NULL,
    [IsActive]              BIT           NOT NULL,
    [IsFullFledgedCustomer] BIT           NOT NULL,
    [CreatedBy]             INT           NOT NULL,
    [CreatedOn]             DATETIME      NOT NULL,
    [ModifiedBy]            INT           NULL,
    [ModifiedOn]            DATETIME      NULL,
    [IsUploadShipment]      BIT           NOT NULL,
    [IsWaitingTimeRequired] BIT           NOT NULL
);

