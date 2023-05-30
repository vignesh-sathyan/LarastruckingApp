CREATE TABLE [dbo].[tblCustomerRegistration] (
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
    [IsUploadShipment]      BIT           CONSTRAINT [DF_tblCustomerRegistration_IsUploadCustomer] DEFAULT ((0)) NOT NULL,
    [IsWaitingTimeRequired] BIT           CONSTRAINT [DF_tblCustomerRegistration_IsWaitingTimeRequired] DEFAULT ((0)) NOT NULL,
    [IsTemperatureRequired] BIT           CONSTRAINT [DF_tblCustomerRegistration_IsTemperatureRequired] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__tblCusto__A4AE64B88A3783EF] PRIMARY KEY CLUSTERED ([CustomerID] ASC),
    CONSTRAINT [FK_tblCustomerRegistration_tblUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[tblUser] ([Userid])
);
















GO
CREATE NONCLUSTERED INDEX [IXCustomerRegistration]
    ON [dbo].[tblCustomerRegistration]([CustomerID] ASC, [CustomerName] ASC, [IsDeleted] ASC);

