CREATE TABLE [dbo].[tblAccidentReport] (
    [AccidentReportId] INT            IDENTITY (1, 1) NOT NULL,
    [EquipmentId]      INT            NULL,
    [DriverId]         INT            NULL,
    [AccidentDate]     DATETIME       NULL,
    [Address]          NVARCHAR (100) NULL,
    [AccidentTime]     NVARCHAR (10)  NULL,
    [Comments]         NVARCHAR (MAX) NULL,
    [IsActive]         BIT            CONSTRAINT [DF__tblAccide__IsAct__2C88998B] DEFAULT ((1)) NOT NULL,
    [CreatedDate]      DATETIME       CONSTRAINT [DF__tblAccide__Creat__2D7CBDC4] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        INT            NOT NULL,
    [ModifiedDate]     DATETIME       NULL,
    [ModifiedBy]       INT            CONSTRAINT [DF__tblAccide__Modif__2E70E1FD] DEFAULT ((0)) NULL,
    [IsDeleted]        BIT            CONSTRAINT [DF__tblAccide__IsDel__2F650636] DEFAULT ((0)) NOT NULL,
    [PoliceReportNo]   VARCHAR (20)   NULL,
    CONSTRAINT [PK__tblAccid__265D95CDA57DCD21] PRIMARY KEY CLUSTERED ([AccidentReportId] ASC),
    CONSTRAINT [FK__tblAccide__Drive__30592A6F] FOREIGN KEY ([DriverId]) REFERENCES [dbo].[tblDriver] ([DriverID]),
    CONSTRAINT [FK__tblAccide__Equip__314D4EA8] FOREIGN KEY ([EquipmentId]) REFERENCES [dbo].[tblEquipmentDetail] ([EDID])
);



