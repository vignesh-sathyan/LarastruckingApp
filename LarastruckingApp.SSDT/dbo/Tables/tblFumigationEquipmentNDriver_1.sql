CREATE TABLE [dbo].[tblFumigationEquipmentNDriver] (
    [FumigationEquipmentNDriverId] INT IDENTITY (1, 1) NOT NULL,
    [FumigationId]                 INT NULL,
    [FumigationRoutsId]            INT NULL,
    [EquipmentId]                  INT NULL,
    [DriverId]                     INT NULL,
    [IsPickUp]                     BIT CONSTRAINT [DF__tblFumiga__IsPic__1209AD79] DEFAULT ((1)) NULL,
    [IsDeleted]                    BIT CONSTRAINT [DF__tblFumiga__IsDel__12FDD1B2] DEFAULT ((0)) NULL,
    CONSTRAINT [PK__tblFumig__10492996948C2ADE] PRIMARY KEY CLUSTERED ([FumigationEquipmentNDriverId] ASC),
    CONSTRAINT [FK__tblFumiga__Fumig__0E04126B] FOREIGN KEY ([FumigationId]) REFERENCES [dbo].[tblFumigation] ([FumigationId]),
    CONSTRAINT [FK__tblFumiga__Fumig__37E53D9E] FOREIGN KEY ([FumigationRoutsId]) REFERENCES [dbo].[tblFumigationRouts] ([FumigationRoutsId])
);












GO
CREATE NONCLUSTERED INDEX [ix_tblFumigationEquipmentNDriver]
    ON [dbo].[tblFumigationEquipmentNDriver]([FumigationId] ASC, [FumigationRoutsId] ASC, [EquipmentId] ASC, [DriverId] ASC, [IsPickUp] ASC, [IsDeleted] ASC);

