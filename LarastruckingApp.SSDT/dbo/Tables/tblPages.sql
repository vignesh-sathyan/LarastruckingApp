CREATE TABLE [dbo].[tblPages] (
    [PageId]         INT           IDENTITY (1, 1) NOT NULL,
    [PageName]       VARCHAR (100) NOT NULL,
    [ControllerName] VARCHAR (100) NULL,
    [ActionName]     VARCHAR (100) NULL,
    [IsActive]       NCHAR (10)    NOT NULL,
    CONSTRAINT [PK_tblPages] PRIMARY KEY CLUSTERED ([PageId] ASC)
);

