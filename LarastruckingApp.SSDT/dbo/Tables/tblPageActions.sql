CREATE TABLE [dbo].[tblPageActions] (
    [PageActionId]   INT          IDENTITY (1, 1) NOT NULL,
    [AreaName]       VARCHAR (50) NULL,
    [ControllerName] VARCHAR (50) NULL,
    [ActionName]     VARCHAR (50) NULL,
    [FeatureId]      INT          NOT NULL,
    [IsActive]       BIT          DEFAULT ((1)) NOT NULL,
    [DisplayOrder]   INT          NULL,
    [IsMenu]         BIT          CONSTRAINT [DF_tblPageActions_IsMenu] DEFAULT ((0)) NOT NULL,
    [DisplayIcon]    VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([PageActionId] ASC)
);

