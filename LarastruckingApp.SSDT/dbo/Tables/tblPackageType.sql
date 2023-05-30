CREATE TABLE [dbo].[tblPackageType] (
    [PackageTypeID]   INT           IDENTITY (1, 1) NOT NULL,
    [PackageTypeName] VARCHAR (300) NULL,
    [CreatedBy]       BIGINT        NULL,
    [CreatedOn]       DATETIME      NULL,
    [ModifiedBy]      BIGINT        NULL,
    [ModifiedOn]      DATETIME      NULL,
    [IsDeleted]       BIT           NULL,
    PRIMARY KEY CLUSTERED ([PackageTypeID] ASC)
);

