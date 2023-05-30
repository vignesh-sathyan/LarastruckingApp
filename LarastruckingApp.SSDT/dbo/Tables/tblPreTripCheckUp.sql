CREATE TABLE [dbo].[tblPreTripCheckUp] (
    [PreTripCheckupId] INT           IDENTITY (1, 1) NOT NULL,
    [ShipmentId]       INT           NOT NULL,
    [EquipmentId]      INT           NOT NULL,
    [UserId]           INT           NOT NULL,
    [IsTiresGood]      BIT           NULL,
    [IsBreaksGood]     BIT           NULL,
    [Fuel]             VARCHAR (50)  NULL,
    [LoadStraps]       VARCHAR (50)  NULL,
    [OverAllCondition] VARCHAR (100) NULL,
    [CreatedOn]        DATETIME      NOT NULL,
    [ModifiedOn]       DATETIME      NOT NULL,
    CONSTRAINT [PK_tblPreTripCheckUp] PRIMARY KEY CLUSTERED ([PreTripCheckupId] ASC),
    CONSTRAINT [FK_tblPreTripCheckUp_tblEquipmentDetail] FOREIGN KEY ([EquipmentId]) REFERENCES [dbo].[tblEquipmentDetail] ([EDID]),
    CONSTRAINT [FK_tblPreTripCheckUp_tblShipment] FOREIGN KEY ([ShipmentId]) REFERENCES [dbo].[tblShipment] ([ShipmentId]),
    CONSTRAINT [FK_tblPreTripCheckUp_tblUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[tblUser] ([Userid])
);





