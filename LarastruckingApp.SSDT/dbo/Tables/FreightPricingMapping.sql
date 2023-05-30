CREATE TABLE [dbo].[FreightPricingMapping] (
    [MappingId]       INT IDENTITY (1, 1) NOT NULL,
    [FreightTypeId]   INT NULL,
    [PricingMethodId] INT NULL,
    [IsActive]        BIT DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([MappingId] ASC)
);

