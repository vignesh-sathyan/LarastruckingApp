CREATE TABLE [dbo].[tblFumigationTypes] (
    [FumigationTypeId] INT           IDENTITY (1, 1) NOT NULL,
    [FumigationName]   VARCHAR (100) NULL,
    [IsActive]         BIT           DEFAULT ((1)) NULL,
    [IsDeleted]        BIT           DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([FumigationTypeId] ASC)
);

