CREATE TABLE [dbo].[tblFreightType] (
    [FreightTypeId]   INT          IDENTITY (1, 1) NOT NULL,
    [FreightTypeName] VARCHAR (50) NULL,
    [IsActive]        BIT          DEFAULT ((1)) NOT NULL,
    [IsDeleted]       BIT          DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([FreightTypeId] ASC)
);

