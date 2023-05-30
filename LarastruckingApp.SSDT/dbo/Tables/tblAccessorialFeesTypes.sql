CREATE TABLE [dbo].[tblAccessorialFeesTypes] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (50) NULL,
    [PricingMethod] VARCHAR (50)  NULL,
    [DisplayOrder]  INT           NULL,
    [IsActive]      BIT           CONSTRAINT [DF__tblAccess__IsAct__6CC31A31] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__tblAcces__3214EC07AB8A2B84] PRIMARY KEY CLUSTERED ([Id] ASC)
);

