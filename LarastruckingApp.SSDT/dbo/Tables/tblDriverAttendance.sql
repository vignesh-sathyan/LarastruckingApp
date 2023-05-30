CREATE TABLE [dbo].[tblTimeCard] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [UserId]      INT          NOT NULL,
    [EquipmentId] INT          NULL,
    [InDateTime]  DATETIME     NULL,
    [OutDateTime] DATETIME     NULL,
    [CreatedBy]   INT          NULL,
    [Day]         VARCHAR (15) NULL,
    [CreatedOn]   DATETIME     NOT NULL,
    CONSTRAINT [PK__tblTimeC__3214EC07973E3D04] PRIMARY KEY CLUSTERED ([Id] ASC)
);







