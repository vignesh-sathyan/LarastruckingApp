CREATE TABLE [dbo].[tblCity] (
    [ID]      INT           IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (50) NULL,
    [StateID] INT           NULL,
    CONSTRAINT [PK_CityMaster] PRIMARY KEY CLUSTERED ([ID] ASC)
);

