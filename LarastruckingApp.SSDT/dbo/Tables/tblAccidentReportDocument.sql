CREATE TABLE [dbo].[tblAccidentReportDocument] (
    [DocumentId]         INT            IDENTITY (1, 1) NOT NULL,
    [AccidentReportId]   INT            NULL,
    [AccidentDocumentId] INT            NULL,
    [ImageURL]           NVARCHAR (MAX) NULL,
    [ImageName]          NVARCHAR (200) NULL,
    [IsActive]           BIT            CONSTRAINT [DF__tblAccide__IsAct__3429BB53] DEFAULT ((1)) NOT NULL,
    [CreatedDate]        DATETIME       CONSTRAINT [DF__tblAccide__Creat__351DDF8C] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]          INT            NOT NULL,
    [ModifiedDate]       DATETIME       NULL,
    [ModifiedBy]         INT            CONSTRAINT [DF__tblAccide__Modif__361203C5] DEFAULT ((0)) NOT NULL,
    [IsDeleted]          BIT            CONSTRAINT [DF__tblAccide__IsDel__370627FE] DEFAULT ((0)) NOT NULL,
    [DocumentName]       NVARCHAR (100) NULL,
    CONSTRAINT [PK__tblAccid__1ABEEF0FB12EEF52] PRIMARY KEY CLUSTERED ([DocumentId] ASC),
    CONSTRAINT [FK__tblAccide__Accid__37FA4C37] FOREIGN KEY ([AccidentReportId]) REFERENCES [dbo].[tblAccidentReport] ([AccidentReportId]),
    CONSTRAINT [FK__tblAccide__Accid__38EE7070] FOREIGN KEY ([AccidentDocumentId]) REFERENCES [dbo].[tblAccidentDocument] ([AccidentDocumentId])
);

