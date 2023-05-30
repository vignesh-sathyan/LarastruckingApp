CREATE TABLE [dbo].[tblDriverDocument] (
    [DocumentId]         INT            IDENTITY (1, 1) NOT NULL,
    [DriverId]           INT            NULL,
    [DocumentTypeId]     INT            NULL,
    [DocumentName]       NVARCHAR (100) NULL,
    [DocumentIssueDate]  DATETIME       NULL,
    [DocumentExpiryDate] DATETIME       NULL,
    [ImageURL]           NVARCHAR (MAX) NULL,
    [ImageName]          NVARCHAR (200) NULL,
    [IsActive]           BIT            DEFAULT ((1)) NOT NULL,
    [CreatedDate]        DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedBy]          INT            NOT NULL,
    [ModifiedDate]       DATETIME       NULL,
    [IsDeleted]          BIT            DEFAULT ((0)) NOT NULL,
    [ModifiedBy]         INT            NULL,
    [EmailSentDate]      DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([DocumentId] ASC),
    FOREIGN KEY ([DocumentTypeId]) REFERENCES [dbo].[tblDocumentType] ([ID]),
    FOREIGN KEY ([DocumentTypeId]) REFERENCES [dbo].[tblDocumentType] ([ID]),
    CONSTRAINT [FK__tblDriver__Drive__075714DC] FOREIGN KEY ([DriverId]) REFERENCES [dbo].[tblDriver] ([DriverID])
);









