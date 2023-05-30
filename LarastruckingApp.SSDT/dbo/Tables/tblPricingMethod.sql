CREATE TABLE [dbo].[tblPricingMethod] (
    [PricingMethodId]   INT          IDENTITY (1, 1) NOT NULL,
    [PricingMethodName] VARCHAR (50) NULL,
    [IsActive]          BIT          DEFAULT ((1)) NOT NULL,
    [IsDeleted]         BIT          DEFAULT ((0)) NOT NULL,
    [PricingMethodExt]  VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([PricingMethodId] ASC)
);



