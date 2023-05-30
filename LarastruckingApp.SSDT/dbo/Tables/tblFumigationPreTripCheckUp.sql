CREATE TABLE [dbo].[tblFumigationPreTripCheckUp] (
    [FumigationPreTripCheckupId] INT           IDENTITY (1, 1) NOT NULL,
    [FumigationId]               INT           NOT NULL,
    [EquipmentId]                INT           NOT NULL,
    [UserId]                     INT           NOT NULL,
    [IsTiresGood]                BIT           NULL,
    [IsBreaksGood]               BIT           NULL,
    [Fuel]                       VARCHAR (50)  NULL,
    [LoadStraps]                 VARCHAR (50)  NULL,
    [OverAllCondition]           VARCHAR (100) NULL,
    [CreatedOn]                  DATETIME      NOT NULL,
    [ModifiedOn]                 DATETIME      NOT NULL,
    CONSTRAINT [PK_tblFumigationPreTripCheckUp] PRIMARY KEY CLUSTERED ([FumigationPreTripCheckupId] ASC)
);

