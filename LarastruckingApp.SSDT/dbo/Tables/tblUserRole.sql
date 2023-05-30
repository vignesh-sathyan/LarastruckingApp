CREATE TABLE [dbo].[tblUserRole] (
    [UserRoleID] INT      IDENTITY (1, 1) NOT NULL,
    [UserID]     INT      NULL,
    [RoleID]     INT      NULL,
    [CreatedBy]  BIGINT   NULL,
    [CreatedOn]  DATETIME NULL,
    [ModifiedBy] BIGINT   NULL,
    [ModifiedOn] DATETIME NULL,
    CONSTRAINT [PK__tblUserR__3D978A55FF20A31C] PRIMARY KEY CLUSTERED ([UserRoleID] ASC),
    CONSTRAINT [FK__tblUserRo__RoleI__41EDCAC5] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[tblRole] ([RoleID]),
    CONSTRAINT [FK__tblUserRo__UserI__40F9A68C] FOREIGN KEY ([UserID]) REFERENCES [dbo].[tblUser] ([Userid])
);

