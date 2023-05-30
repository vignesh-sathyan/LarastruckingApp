CREATE TABLE [dbo].[tblPageAuthorization] (
    [Id]              INT IDENTITY (1, 1) NOT NULL,
    [RoleId]          INT NOT NULL,
    [PageId]          INT NOT NULL,
    [CanView]         BIT CONSTRAINT [DF_tblPageAuthorization_CanView] DEFAULT ((0)) NOT NULL,
    [CanInsert]       BIT CONSTRAINT [DF_tblPageAuthorization_CanEdit] DEFAULT ((0)) NOT NULL,
    [CanUpdate]       BIT CONSTRAINT [DF_tblPageAuthorization_CanUpdate] DEFAULT ((0)) NOT NULL,
    [CanDelete]       BIT CONSTRAINT [DF_tblPageAuthorization_CanDelete] DEFAULT ((0)) NOT NULL,
    [IsPricingMethod] BIT CONSTRAINT [DF_tblPageAuthorization_CanPricingMethod] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_tblPageAuthorization] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_tblPageAuthorization_tblPages] FOREIGN KEY ([PageId]) REFERENCES [dbo].[tblPages] ([PageId]),
    CONSTRAINT [FK_tblPageAuthorization_tblRole] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[tblRole] ([RoleID])
);



