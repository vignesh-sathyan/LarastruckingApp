CREATE TABLE [dbo].[tblTimeCardlog] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [UserId]       INT           NOT NULL,
    [EquipmentId]  INT           NULL,
    [IsCheckIn]    BIT           NOT NULL,
    [ScanDateTime] DATETIME      NULL,
    [CreatedBy]    INT           NULL,
    [CreatedOn]    DATETIME      NOT NULL,
    [Latitude]     VARCHAR (MAX) NULL,
    [Longitude]    VARCHAR (50)  NULL,
    [IsSuccess]    BIT           NULL,
    CONSTRAINT [PK__tblDrive__3214EC07C242CCBA] PRIMARY KEY CLUSTERED ([Id] ASC)
);





