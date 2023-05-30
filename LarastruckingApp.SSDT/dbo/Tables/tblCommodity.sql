CREATE TABLE [dbo].[tblCommodity] (
    [CommodityId]   INT          IDENTITY (1, 1) NOT NULL,
    [CommodityName] VARCHAR (50) NULL,
    [IsActive]      BIT          DEFAULT ((1)) NOT NULL,
    [IsDeleted]     BIT          DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([CommodityId] ASC)
);

