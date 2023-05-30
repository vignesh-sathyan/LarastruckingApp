CREATE TABLE [dbo].[tblRole] (
    [RoleID]     INT           IDENTITY (1, 1) NOT NULL,
    [RoleName]   VARCHAR (300) NULL,
    [CreatedBy]  BIGINT        NULL,
    [CreatedOn]  DATETIME      NULL,
    [ModifiedBy] BIGINT        NULL,
    [ModifiedOn] DATETIME      NULL,
    [IsActive]   BIT           NULL,
    [IsDeleted]  BIT           NULL,
    PRIMARY KEY CLUSTERED ([RoleID] ASC)
);

