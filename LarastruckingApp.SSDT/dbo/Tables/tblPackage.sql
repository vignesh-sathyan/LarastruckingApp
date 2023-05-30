CREATE TABLE [dbo].[tblPackage] (
    [PackageID]    BIGINT          IDENTITY (1, 1) NOT NULL,
    [PackageType]  INT             NULL,
    [Quantity]     DECIMAL (18, 2) NULL,
    [FreightType]  BIGINT          NULL,
    [IsHazardus]   BIT             NULL,
    [PackageRefNo] VARCHAR (MAX)   NULL,
    [ShipmentID]   BIGINT          NULL,
    [CreatedBy]    BIGINT          NULL,
    [CreatedOn]    DATETIME        NULL,
    [ModifiedBy]   BIGINT          NULL,
    [ModifiedOn]   DATETIME        NULL,
    [IsDeleted]    BIT             NULL,
    CONSTRAINT [PK_tblPackage] PRIMARY KEY CLUSTERED ([PackageID] ASC),
    FOREIGN KEY ([PackageType]) REFERENCES [dbo].[tblPackageType] ([PackageTypeID]),
    FOREIGN KEY ([PackageType]) REFERENCES [dbo].[tblPackageType] ([PackageTypeID])
);





