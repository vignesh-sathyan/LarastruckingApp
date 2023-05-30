CREATE TABLE [dbo].[tblAddressType] (
    [AddressTypeID]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [AddressTypeName] VARCHAR (300) NULL,
    [CreatedBy]       BIGINT        NULL,
    [CreatedOn]       DATETIME      NULL,
    [ModifiedBy]      BIGINT        NULL,
    [ModifiedOn]      DATETIME      NULL,
    [IsDeleted]       BIT           NULL,
    PRIMARY KEY CLUSTERED ([AddressTypeID] ASC)
);

