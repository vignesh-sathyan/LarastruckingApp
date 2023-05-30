CREATE TABLE [dbo].[tblLanguage] (
    [LanguageId] INT          IDENTITY (1, 1) NOT NULL,
    [Language]   VARCHAR (20) NULL,
    [IsActive]   BIT          CONSTRAINT [DF_tblLanguage_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__tblLangu__B93855AB0CA7F09C] PRIMARY KEY CLUSTERED ([LanguageId] ASC)
);

